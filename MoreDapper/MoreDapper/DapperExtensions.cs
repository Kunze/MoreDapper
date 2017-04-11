using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using MoreDapper.CommandGenerator;

namespace MoreDapper
{
    public static class DapperExtensions
    {
        public static int Insert<T>(this System.Data.IDbConnection connection, T param, string table = null, int? commandTimeout = null, IDbTransaction transaction = null) where T:class
        {
            if(param == null)
            {
                throw new ArgumentNullException("param can not be null.");
            }

            var command = new InsertGenerator().Generate(param, table);

            return connection.Execute(command, param, commandTimeout: commandTimeout, transaction: transaction);
        }

        public static int Update<T>(this System.Data.IDbConnection connection, T param, string table = null, int? commandTimeout = null, IDbTransaction transaction = null)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param can not be null.");
            }

            var command = new UpdateGenerator().Generate(param, table);

            return connection.Execute(command, param, commandTimeout: commandTimeout, transaction: transaction);
        }

        public static int Delete<T>(this System.Data.IDbConnection connection, T param, string table = null, int? commandTimeout = null, IDbTransaction transaction = null)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param can not be null.");
            }

            var command = new DeleteGenerator().Generate(param, table);

            return connection.Execute(command, param, commandTimeout: commandTimeout, transaction: transaction);
        }

        /// <summary>
        /// Execute insert command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="insert">INSERT command</param>
        /// <param name="values">sql values ex: (@Foo, @Bar, 'Bar', 123, @Foo2)</param>
        /// <param name="list">IList<T> objects</param>
        /// <param name="maxItens">Maximun values per execution, ex: insert into table values (@Foo), (@Foo), (@Foo)</param>
        /// <returns>Total numbers affected</returns>
        public static int Insert<T>(this IDbConnection connection, string insert, string values, IList<T> list, int maxItens = 1000, int? commandTimeout = null, IDbTransaction transaction = null)
        {
            if (string.IsNullOrWhiteSpace(insert))
            {
                throw new ArgumentNullException("insert can not be null or white space.");
            }

            if (string.IsNullOrWhiteSpace(values))
            {
                throw new ArgumentNullException("values can not be null or white space.");
            }

            if(list == null)
            {
                throw new ArgumentNullException("list can not be null.");
            }

            var commands = new List<string>();
            foreach (var listItems in list.Select((x, i) => new { Index = i, Value = x })
             .GroupBy(x => x.Index / maxItens)
             .Select(x => x.Select(v => v.Value).ToList())
             .ToList())
            {
                var command = new InsertGenerator().Generate(insert, values, listItems);
                commands.Add(command);

                //<TODO>Ao enviar tudo em um comando só: Packets larger than max_allowed_packet are not allowed.
                //total += connection.Execute(string.Concat(insert, command));
            }

            var total = 0;
            foreach (var command in commands)
            {
                total += connection.Execute(command, commandTimeout: commandTimeout, transaction: transaction);
            }

            return total;
        }
    }
}
