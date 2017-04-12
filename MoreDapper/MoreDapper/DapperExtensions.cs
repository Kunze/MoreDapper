using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using MoreDapper.CommandGenerator;
using System.Collections;

namespace MoreDapper
{
    public static class DapperExtensions
    {
        public static int InsertSingle<T>(this System.Data.IDbConnection connection, T param, string table = null, int? commandTimeout = null, IDbTransaction transaction = null)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param can not be null.");
            }

            if (param is IEnumerable)
            {
                throw new ArgumentException("param can not be a IEnumerable. Call InsertMultiple instead.");
            }

            var command = new InsertGenerator().GenerateSingle(param, table);

            return connection.Execute(command, param, commandTimeout: commandTimeout, transaction: transaction);
        }

        public static int InsertMultiple<T>(this System.Data.IDbConnection connection, IList<T> list, string table = null, int maxItens = 1000, int maxPacketSize = 4194304, int? commandTimeout = null, IDbTransaction transaction = null)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list can not be null.");
            }

            if (maxItens < 0)
            {
                throw new ArgumentException("maxItens can not be less than 0.");
            }

            var commands = new InsertGenerator().GenerateMultiple(list, maxItens, maxPacketSize, table);

            var total = 0;
            foreach (var command in commands)
            {
                total += connection.Execute(command, commandTimeout: commandTimeout, transaction: transaction);
            }

            return total;
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
        public static int InsertMultiple<T>(this IDbConnection connection, string insert, string values, IList<T> list, int maxItens = 1000, int maxPacketSize = 4194304, int? commandTimeout = null, IDbTransaction transaction = null)
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

            if (maxItens < 0)
            {
                throw new ArgumentException("maxItens can not be less than 0.");
            }

            var commands = new InsertGenerator().GenerateMultiple(insert, values, list, maxItens, maxPacketSize);

            var total = 0;
            foreach (var command in commands)
            {
                total += connection.Execute(command, commandTimeout: commandTimeout, transaction: transaction);
            }

            return total;
        }

        public static int UpdateSingle<T>(this System.Data.IDbConnection connection, T param, string table = null, int? commandTimeout = null, IDbTransaction transaction = null)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param can not be null.");
            }

            if (param is IEnumerable)
            {
                throw new ArgumentException("param can not be a IEnumerable.");
            }

            var command = new UpdateGenerator().Generate(param, table);

            return connection.Execute(command, param, commandTimeout: commandTimeout, transaction: transaction);
        }

        public static int DeleteSingle<T>(this System.Data.IDbConnection connection, T param, string table = null, int? commandTimeout = null, IDbTransaction transaction = null)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param can not be null.");
            }

            if (param is IEnumerable)
            {
                throw new ArgumentException("param can not be a IEnumerable.");
            }

            var command = new DeleteGenerator().Generate(param, table);

            return connection.Execute(command, param, commandTimeout: commandTimeout, transaction: transaction);
        }
    }
}
