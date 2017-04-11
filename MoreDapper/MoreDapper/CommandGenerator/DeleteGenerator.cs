using MoreDapper.Config;
using MoreDapper.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreDapper.CommandGenerator
{
    internal class DeleteGenerator
    {
        internal string Generate<T>(T param, string table = null)
        {
            if(param == null)
            {
                throw new ArgumentNullException("param can not be null.");
            }

            var type = typeof(T);
            var identityProperties = MoreDapperConfig.GetKeysFor(type);

            if (identityProperties.Count == 0)
            {
                throw new PrimaryKeyNotFoundException($"type {type.FullName} does not have a primary key.");
            }

            var tableName = table ?? type.Name;
            var where = new List<string>();

            foreach (var identityProperty in identityProperties)
            {
                where.Add($"{identityProperty} = @{identityProperty}");
            }

            return string.Concat($"DELETE FROM {tableName} WHERE ", string.Join(" AND ", where), ";");
        }
    }
}
