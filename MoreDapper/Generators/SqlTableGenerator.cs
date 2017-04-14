using MoreDapper.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreDapper.Generators
{
    internal class SqlValue
    {
        internal string Name { get; set; }
        internal string Type { get; set; }
    }

    internal class SqlTable
    {
        internal string Name { get; set; }
        internal List<SqlValue> Values { get; set; }
        internal List<SqlValue> Keys { get; set; }

        internal SqlTable(string name)
        {
            Name = name;
            Values = new List<SqlValue>();
            Keys = new List<SqlValue>();
        }
    }

    internal static class SqlTableGenerator
    {
        internal static SqlTable Generate<T>(T param)
        {
            var type = typeof(T);
            var table = type.Name;
            var keys = MoreDapperConfig.GetKeysFor(type);
            var properties = type.GetProperties().Where(p => !keys.Contains(p.Name)).ToList();
            var converter = SqlTypeConverter.GetInstance();
            var sqlTable = new SqlTable(table);

            foreach (var property in properties)
            {
                sqlTable.Values.Add(new SqlValue
                {
                    Name = property.Name,
                    Type = converter.GetPropertyType(param, property)
                });
            }

            foreach (var key in keys)
            {
                var property = type.GetProperty(key);

                sqlTable.Keys.Add(new SqlValue
                {
                    Name = property.Name,
                    Type = converter.GetPropertyType(param, property)
                });
            }

            return sqlTable;
        }
    }
}
