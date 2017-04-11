using MoreDapper.Config;
using MoreDapper.Scanner;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreDapper
{
    internal class InsertGenerator
    {
        internal string GenerateSingle<T>(T param, string table = null)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param can not be null.");
            }

            var type = typeof(T);
            var tableName = table ?? type.Name;
            var identityProperties = MoreDapperConfig.GetKeysFor(type);
            var properties = type.GetProperties().Where(p => !identityProperties.Contains(p.Name)).ToList();
            var converter = SqlTypeConverter.GetInstance();
            var insert = new List<string>();
            var values = new List<string>();

            foreach (var property in properties)
            {
                insert.Add($"{property.Name}");
                values.Add($"@{property.Name}");
            }

            return string.Concat($"INSERT INTO {tableName} (", string.Join(", ", insert), ") ", "VALUES (", string.Join(", ", values), ");");
        }

        internal string GenerateMultiple<T>(IList<T> list, string table = null)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list can not be null.");
            }

            var type = typeof(T);
            var tableName = table ?? type.Name;
            var identityProperties = MoreDapperConfig.GetKeysFor(type);
            var properties = type.GetProperties().Where(p => !identityProperties.Contains(p.Name)).ToList();
            var converter = SqlTypeConverter.GetInstance();
            var insert = new StringBuilder($"INSERT INTO {tableName} (");
            var values = new StringBuilder();

            for (int i = 0; i < properties.Count; i++)
            {
                var property = properties[i];

                insert.Append($"{property.Name}");

                if (i != properties.Count - 1)
                {
                    insert.Append(", ");
                }
            }

            for (int listIndex = 0; listIndex < list.Count; listIndex++)
            {
                var item = list[listIndex];

                values.Append("(");
                for (int propertyIndex = 0; propertyIndex < properties.Count; propertyIndex++)
                {
                    var property = properties[propertyIndex];
                    var value = converter.GetValue(item, property);

                    values.Append($"{value}");

                    if (propertyIndex != properties.Count - 1)
                    {
                        values.Append(", ");
                    }
                }
                values.Append(")");

                if (listIndex != list.Count - 1)
                {
                    values.Append(", ");
                }
            }

            return string.Concat(insert, ") VALUES ", string.Join(", ", values), ";");
        }

        internal string GenerateMultiple<T>(string insert, string values, IList<T> list)
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

            if (values[0] == '(')
            {
                values = values.Remove(0, 1);
            }

            if (values[values.Length - 1] == ')')
            {
                values = values.Remove(values.Length - 1, 1);
            }

            var type = typeof(T);
            var properties = type.GetProperties(); //<TODO>não funciona com Fields
            var sqlProperties = SqlParameterScanner.Scan(values); //<TODO> gerar arvore sintática e gerar comando com string.Join
            var converter = SqlTypeConverter.GetInstance();
            var listValues = new StringBuilder();

            for (int listIndex = 0; listIndex < list.Count; listIndex++)
            {
                var sql = values;
                var item = list[listIndex];
                var addIndex = 0;

                for (int propertyIndex = 0; propertyIndex < sqlProperties.Count; propertyIndex++)
                {
                    var parameter = sqlProperties[propertyIndex];
                    var value = converter.GetValue(item, type.GetProperty(parameter.GetParameterName()));
                    var dif = (value.Length - parameter.Name.Length);

                    sql = sql.Remove(parameter.StartIndex + addIndex, parameter.Name.Length);
                    sql = sql.Insert(parameter.StartIndex + addIndex, value);

                    addIndex += dif;
                }

                listValues.Append($"({sql})");

                if (listIndex != list.Count - 1)
                {
                    listValues.Append(", ");
                }
            }

            return string.Concat(insert, " ", listValues, ";");
        }
    }
}
