using MoreDapper.Config;
using MoreDapper.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreDapper.CommandGenerator
{
    internal static class UpdateGenerator
    {
        internal static string Generate<T>(T param, string table = null)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param can not be null.");
            }

            var type = typeof(T);
            var identityProperties = MoreDapperConfig.GetKeysFor(type);

            if(identityProperties.Count == 0)
            {
                throw new PrimaryKeyNotFoundException($"type {type.FullName} does not have a auto identity property.");
            }

            var tableName = table ?? type.Name;
            var properties = type.GetProperties().Where(p => !identityProperties.Contains(p.Name)).ToList();
            var converter = SqlTypeConverter.GetInstance();
            var declares = new StringBuilder();
            var set = new List<string>();
            var where = new List<string>();

            foreach (var property in properties)
            {
                set.Add($"{property.Name} = @{property.Name}");
            }

            foreach (var identityProperty in identityProperties)
            {
                where.Add($"{identityProperty} = @{identityProperty}");
            }

            return string.Concat($"UPDATE {tableName} SET ", string.Join(", ", set), " WHERE ", string.Join(" AND ", where), ";");
        }
    }
}
