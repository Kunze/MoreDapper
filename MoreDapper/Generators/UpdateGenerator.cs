using MoreDapper.Config;
using MoreDapper.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreDapper.Generators
{
    internal static class UpdateGenerator
    {
        internal static string Generate<T>(T param)
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

            var sqlTable = SqlTableGenerator.Generate(param);
            var declares = new StringBuilder();
            var set = new List<string>();
            var where = new List<string>();

            foreach (var property in sqlTable.Values)
            {
                set.Add($"{property.Name} = @{property.Name}");
            }

            foreach (var identityProperty in sqlTable.Keys)
            {
                where.Add($"{identityProperty.Name} = @{identityProperty.Name}");
            }

            return string.Concat($"UPDATE {sqlTable.Name} SET ", string.Join(", ", set), " WHERE ", string.Join(" AND ", where), ";");
        }
    }
}
