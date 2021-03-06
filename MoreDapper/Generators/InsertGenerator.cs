﻿using MoreDapper.Generators;
using MoreDapper.Config;
using MoreDapper.Exceptions;
using MoreDapper.Scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreDapper.Generators
{
    internal static class InsertGenerator
    {
        internal static string GenerateSingle<T>(T param)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param can not be null.");
            }

            var type = typeof(T);
            var sqlTable = SqlTableGenerator.Generate(param);
            var insert = new List<string>();
            var values = new List<string>();

            foreach (var property in sqlTable.Values)
            {
                insert.Add($"{property.Name}");
                values.Add($"@{property.Name}");
            }

            return string.Concat($"INSERT INTO {sqlTable.Name} (", string.Join(", ", insert), ") ", "VALUES (", string.Join(", ", values), ");");
        }

        internal static List<string> GenerateMany<T>(IList<T> list, int maxItens, int maxPacketSize)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list can not be null.");
            }

            if (maxItens <= 0)
            {
                throw new ArgumentException("maxItens can not be less or equal 0");
            }

            if (maxPacketSize <= 0)
            {
                throw new ArgumentException("maxPacketSize can not be less or equal 0");
            }

            var type = typeof(T);
            var tableName = type.Name;
            var identityProperties = MoreDapperConfig.GetKeysFor(type);
            var properties = type.GetProperties().Where(p => !identityProperties.Contains(p.Name)).ToList();
            var converter = SqlTypeConverter.GetInstance();
            var insert = new StringBuilder($"INSERT INTO {tableName} (");
            var values = new List<string>();

            for (int i = 0; i < properties.Count; i++)
            {
                var property = properties[i];

                insert.Append($"{property.Name}");

                if (i != properties.Count - 1)
                {
                    insert.Append(", ");
                }
            }
            insert.Append(") VALUES ");

            for (int listIndex = 0; listIndex < list.Count; listIndex++)
            {
                var item = list[listIndex];
                var internalValues = new List<string>();

                for (int propertyIndex = 0; propertyIndex < properties.Count; propertyIndex++)
                {
                    var property = properties[propertyIndex];
                    var value = converter.GetValue(item, property);

                    internalValues.Add($"{value}");
                }

                values.Add($"({string.Join(", ", internalValues)})");
            }

            return GenerateMany(insert.ToString(), values, maxItens, maxPacketSize);
        }

        internal static List<string> GenerateMany<T>(string insert, string values, IList<T> list, int maxItens, int maxPacketSize)
        {
            if (string.IsNullOrWhiteSpace(insert))
            {
                throw new ArgumentNullException("insert can not be null or white space.");
            }

            if (string.IsNullOrWhiteSpace(values))
            {
                throw new ArgumentNullException("values can not be null or white space.");
            }

            if (list == null)
            {
                throw new ArgumentNullException("list can not be null.");
            }

            if (maxItens <= 0)
            {
                throw new ArgumentException("maxItens can not be less or equal 0");
            }

            if (maxPacketSize <= 0)
            {
                throw new ArgumentException("maxPacketSize can not be less or equal 0");
            }

            if (values[0] == '(')
            {
                values = values.Remove(0, 1);
            }

            if (values[values.Length - 1] == ')')
            {
                values = values.Remove(values.Length - 1, 1);
            }

            var type = typeof(T);
            var sqlParameters = SqlParameterScanner.Scan(values); //<TODO> gerar arvore sintática e gerar comando com string.Join
            var converter = SqlTypeConverter.GetInstance();
            var listValues = new List<string>();

            foreach (var sqlParameter in sqlParameters)
            {
                var parameterName = sqlParameter.GetParameterName();
                var property = type.GetProperty(parameterName);

                if (property == null)
                {
                    throw new InvalidSqlParameterException($"Property '{parameterName}' does not exist in type {type.Name}.");
                }
            }

            for (int listIndex = 0; listIndex < list.Count; listIndex++)
            {
                var sql = values;
                var item = list[listIndex];
                var addIndex = 0;

                for (int propertyIndex = 0; propertyIndex < sqlParameters.Count; propertyIndex++)
                {
                    var parameter = sqlParameters[propertyIndex];
                    var parameterName = parameter.GetParameterName();
                    var property = type.GetProperty(parameterName);
                    var value = converter.GetValue(item, property);
                    var dif = (value.Length - parameter.Name.Length);

                    sql = sql.Remove(parameter.StartIndex + addIndex, parameter.Name.Length);
                    sql = sql.Insert(parameter.StartIndex + addIndex, value);

                    addIndex += dif;
                }

                listValues.Add($"({sql})");
            }

            return GenerateMany(insert, listValues, maxItens, maxPacketSize);
        }

        private static List<string> GenerateMany(string insert, List<string> values, int maxItens, int maxPacketSize)
        {
            var commands = new List<string>();
            var insertSize = Encoding.UTF8.GetBytes(insert).Length;
            var commandSize = insertSize;
            insert = insert.Trim();
            insert = string.Concat(insert, " ");

            foreach (var listValues in values.Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / maxItens)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList())
            {
                var command = new StringBuilder(insert);

                for (int i = 0; i < listValues.Count; i++)
                {
                    var value = listValues[i];
                    var concatenation = string.Empty;
                    if (i > 0)
                    {
                        concatenation = ", ";
                    }

                    var valueSize = Encoding.UTF8.GetBytes(value).Length;
                    var concatenatedValue = string.Concat(concatenation, value);
                    var concatenatedValueSize = Encoding.UTF8.GetBytes(concatenatedValue).Length;
                    var sumSize = commandSize + concatenatedValueSize;

                    if (sumSize >= maxPacketSize)
                    {
                        commands.Add($"{command.ToString()};");

                        command = new StringBuilder(insert);
                        command.Append(value);
                        commandSize = insertSize + valueSize;
                    }
                    else
                    {
                        commandSize = sumSize;
                        command.Append(concatenatedValue);
                    }
                }

                commands.Add($"{command.ToString()};");
            }

            return commands;
        }
    }
}
