using MoreDapper.Config;
using MoreDapper.Exceptions;
using System;
using System.Collections.Generic;

namespace MoreDapper.Generators
{
    internal static class DeleteGenerator
    {
        internal static string Generate<T>(T param)
        {
            if(param == null)
            {
                throw new ArgumentNullException("param can not be null.");
            }

            var type = typeof(T);
            var sqlTable = SqlTableGenerator.Generate(param);
            if (sqlTable.Keys.Count == 0)
            {
                throw new PrimaryKeyNotFoundException($"type {type.FullName} does not have a primary key.");
            }

            var where = new List<string>();

            foreach (var identityProperty in sqlTable.Keys)
            {
                where.Add($"{identityProperty.Name} = @{identityProperty.Name}");
            }

            return string.Concat($"DELETE FROM {sqlTable.Name} WHERE ", string.Join(" AND ", where), ";");
        }
    }
}
