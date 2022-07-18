using Axiom.Application.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace Axiom.Application.Interfaces.Database
{
    public interface IDapper : IScopedDependency
    {
        public string TransactionConnectionString { get; set; }  
        IDbConnection TransactionConnection { get; }
        IDbTransaction TransactionScope { get; }
        IDbConnection CreateConnection(string connectionString);

        ExpandoObject ExecuteProcedure(string connectionString, string procedureName, List<Tuple<string, string>> arguments, int timeout = 300);

        /// <summary>
        /// to execute sproc query data, return ExpandoObject
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="procedureName"></param>
        /// <param name="arguments"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ExpandoObject> ExecuteProcedureAsync(string connectionString, string procedureName, List<Tuple<string, string>> arguments, int timeout = 300);

        /// <summary>
        /// to execute sproc query data, return List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="procedureName"></param>
        /// <param name="arguments"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<List<T>> GetList<T>(string connectionString, string procedureName, List<Tuple<string, string>> arguments, int timeout = 300);

        /// <summary>
        /// to execute sproc query data, return single row
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="procedureName"></param>
        /// <param name="arguments"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<T> GetSingle<T>(string connectionString, string procedureName, List<Tuple<string, string>> arguments, int timeout = 300);

        /// <summary>
        /// to execute sproc insert/ update/ delete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="procedureName"></param>
        /// <param name="arguments"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<T> ProcedureExecuteWithTransactionAsync<T>(string procedureName, List<Tuple<string, string>> arguments, int timeout = 300);
    }
}
