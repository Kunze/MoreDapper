using System;
using System.Collections.Generic;
using Dapper;
using System.Data;
using MoreDapper.Generators;
using System.Collections;

namespace MoreDapper
{
    public static class DapperExtensions
    {
        /// <summary>
        /// Insert a ingle row
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="param">object</param>
        /// <param name="table">Optional table name</param>
        /// <param name="commandTimeout">commandTimeout</param>
        /// <param name="transaction">transaction</param>
        /// <returns>Numbers of rows affected</returns>
        public static int InsertSingle<T>(this IDbConnection connection, T param, int? commandTimeout = null, IDbTransaction transaction = null)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param can not be null.");
            }

            if (param is IEnumerable)
            {
                throw new ArgumentException("param can not be a IEnumerable. Call InsertMultiple instead.");
            }

            var command = InsertGenerator.GenerateSingle(param);

            return connection.Execute(command, param, commandTimeout: commandTimeout, transaction: transaction);
        }

        /// <summary>
        /// Insert multiples rows
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="list">IList<T> objects</param>
        /// <param name="table">optional table name</param>
        /// <param name="maxItens">Maximun values per execution</param>
        /// <param name="maxPacketSize">Max size of command in bytes</param>
        /// <param name="commandTimeout">commandTimeout</param>
        /// <param name="transaction">transaction</param>
        /// <returns>Numbers of rows affected</returns>
        public static int InsertMultiple<T>(this IDbConnection connection, IList<T> list, int maxItens = 1000, int maxPacketSize = 4194304, int? commandTimeout = null, IDbTransaction transaction = null)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list can not be null.");
            }

            if (maxItens < 0)
            {
                throw new ArgumentException("maxItens can not be less than 0.");
            }

            var commands = InsertGenerator.GenerateMultiple(list, maxItens, maxPacketSize);

            var total = 0;
            foreach (var command in commands)
            {
                total += connection.Execute(command, commandTimeout: commandTimeout, transaction: transaction);
            }

            return total;
        }

        /// <summary>
        /// Insert multiple rows
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="insert">INSERT command "insert into Bar values "</param>
        /// <param name="values">sql values</param>
        /// <param name="list">IList<T> objects</param>
        /// <param name="maxItens">Maximun values per execution</param>
        /// <param name="maxPacketSize">Max size of command in bytes</param>
        /// <param name="commandTimeout">commandTimeout</param>
        /// <param param name="transaction">transaction</param>
        /// <returns>Numbers of rows affected</returns>
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

            var commands = InsertGenerator.GenerateMultiple(insert, values, list, maxItens, maxPacketSize);

            var total = 0;
            foreach (var command in commands)
            {
                total += connection.Execute(command, commandTimeout: commandTimeout, transaction: transaction);
            }

            return total;
        }

        /// <summary>
        /// Update a single row
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="param">object</param>
        /// <param name="table">Optional table name</param>
        /// <param name="commandTimeout">commandTimeout</param>
        /// <param name="transaction">transaction</param>
        /// <returns>Numbers of rows affected</returns>
        public static int UpdateSingle<T>(this IDbConnection connection, T param, int? commandTimeout = null, IDbTransaction transaction = null)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param can not be null.");
            }

            if (param is IEnumerable)
            {
                throw new ArgumentException("param can not be a IEnumerable.");
            }

            var command = UpdateGenerator.Generate(param);

            return connection.Execute(command, param, commandTimeout: commandTimeout, transaction: transaction);
        }

        /// <summary>
        /// Delete a single row
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="param">object</param>
        /// <param name="table">Optional table name</param>
        /// <param name="commandTimeout">commandTimeout</param>
        /// <param name="transaction">transaction</param>
        /// <returns>Numbers of rows affected</returns>
        public static int DeleteSingle<T>(this IDbConnection connection, T param, int? commandTimeout = null, IDbTransaction transaction = null)
        {
            if (param == null)
            {
                throw new ArgumentNullException("param can not be null.");
            }

            if (param is IEnumerable)
            {
                throw new ArgumentException("param can not be a IEnumerable.");
            }

            var command = DeleteGenerator.Generate(param);

            return connection.Execute(command, param, commandTimeout: commandTimeout, transaction: transaction);
        }
    }
}
