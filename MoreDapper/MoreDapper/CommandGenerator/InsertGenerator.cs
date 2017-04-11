using MoreDapper.Config;
using MoreDapper.Scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreDapper
{
    internal class InsertGenerator
    {
        internal string Generate<T>(T param, string table = null)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param can not be null.");
            }

            var type = typeof(T);
            var tableName = table ?? type.Name;
            var identityProperties = MoreDapperConfig.GetPropertiesFor(type);
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

        internal string Generate<T>(string insert, string values, IList<T> list)
        {
            var generatedValues = Generate(values, list);

            return string.Concat(insert, " ", generatedValues, ";");
        }

        internal string Generate<T>(string values, IList<T> list)
        {
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

                if(listIndex != list.Count - 1)
                {
                    listValues.Append(", ");
                }
            }

            return listValues.ToString();
        }
    }
}
