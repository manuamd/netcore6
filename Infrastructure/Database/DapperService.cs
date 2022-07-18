using Axiom.Application;
using Axiom.Application.Extensions;
using Axiom.Application.Interfaces.Database;
using Axiom.Application.Interfaces.Service;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Axiom.Infrastructure.Database
{
    public class DapperService : IDapper, IDisposable, IScopedDependency
    {
        public string TransactionConnectionString { get; set; }
        public IDbConnection TransactionConnection { get { return GetTransactionConnection(); } }
        private IDbConnection _TransactionConnection;
        public IDbTransaction TransactionScope { get { return GetTransactionScope(); } }
        private IDbTransaction _TransactionScope;

        public DapperService()
        {
        }

        public void Dispose()
        {

        }
        public IDbConnection GetTransactionConnection() {
            if (_TransactionConnection is null) {

                _TransactionConnection = new SqlConnection(TransactionConnectionString);
                // Properly initialize your connection here.
            }
            return _TransactionConnection;
        }
        public IDbTransaction GetTransactionScope()
        {
            if (_TransactionScope is null) 
            {
                if (TransactionConnection.State == ConnectionState.Closed)
                {
                    TransactionConnection.Open();
                }

                _TransactionScope = TransactionConnection.BeginTransaction();
            }
            return _TransactionScope;
        }


        public ExpandoObject ExecuteProcedure(string connectionString, string procedureName, List<Tuple<string, string>> arguments, int timeout = 300)
        {
            XDocument xDocument;

            try
            {
                DynamicParameters parameter = new DynamicParameters();
                arguments?.ForEach(arg =>
                {
                    if (arg.Item2 == "-=-" || string.IsNullOrWhiteSpace(arg.Item2))
                    {
                        parameter.Add(arg.Item1, null);
                    }
                    else
                    {
                        parameter.Add(arg.Item1, arg.Item2);
                    }

                });

                var stringBuilder = new StringBuilder();

                try
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        using (var reader = connection.ExecuteReader(procedureName, parameter, commandTimeout: timeout, commandType: CommandType.StoredProcedure))
                        {
                            do
                            {
                                while (reader.Read())
                                {
                                    stringBuilder.Append(reader[0].ToString());
                                }
                            } while (reader.NextResult());

                        }
                    }
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();
                }

                xDocument = XDocument.Parse("<data>" + stringBuilder.ToString() + "</data>");
                var xmlDocument = new XmlDocument();
                using (var xmlReader = xDocument.CreateReader())
                {
                    xmlDocument.Load(xmlReader);
                }

                return xmlDocument.ToObject();
            }
            catch
            {
                return null;
            }
        }


        public async Task<ExpandoObject> ExecuteProcedureAsync(string connectionString, string procedureName, List<Tuple<string, string>> arguments, int timeout = 300)
        {
            XDocument xDocument;

            try
            {
                DynamicParameters parameter = new DynamicParameters();
                arguments?.ForEach(arg =>
                {
                    if (arg.Item2 == "-=-" || string.IsNullOrWhiteSpace(arg.Item2))
                    {
                        parameter.Add(arg.Item1, null);
                    }
                    else
                    {
                        parameter.Add(arg.Item1, arg.Item2);
                    }

                });

                var stringBuilder = new StringBuilder();

                try
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        using (var reader = await connection.ExecuteReaderAsync(procedureName, parameter, commandTimeout: timeout, commandType: CommandType.StoredProcedure))
                        {
                            do
                            {
                                while (reader.Read())
                                {
                                    stringBuilder.Append(reader[0].ToString());
                                }
                            } while (reader.NextResult());

                        }
                    }
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();
                }

                xDocument = XDocument.Parse("<data>" + stringBuilder.ToString() + "</data>");
                var xmlDocument = new XmlDocument();
                using (var xmlReader = xDocument.CreateReader())
                {
                    xmlDocument.Load(xmlReader);
                }

                return xmlDocument.ToObject();
            }
            catch
            {
                return null;
            }
        }

        public async Task<T> ProcedureExecuteWithTransactionAsync<T>(string procedureName, List<Tuple<string, string>> arguments, int timeout = 300)
        {

            DynamicParameters parameter = new DynamicParameters();
            arguments?.ForEach(arg =>
            {
                if (arg.Item2 == "-=-" || string.IsNullOrWhiteSpace(arg.Item2))
                {
                    parameter.Add(arg.Item1, null);
                }
                else
                {
                    parameter.Add(arg.Item1, arg.Item2);
                }

            });

            var result = await TransactionConnection.QueryAsync<T>(procedureName, parameter, commandTimeout: timeout, commandType: CommandType.StoredProcedure, transaction: TransactionScope);
            return result.FirstOrDefault();


        }

        public async Task<List<T>> GetList<T>(string connectionString, string procedureName, List<Tuple<string, string>> arguments, int timeout = 300)
        {
            try
            {
                DynamicParameters parameter = new DynamicParameters();
                arguments?.ForEach(arg =>
                {
                    if (arg.Item2 == "-=-" || string.IsNullOrWhiteSpace(arg.Item2))
                    {
                        parameter.Add(arg.Item1, null);
                    }
                    else
                    {
                        parameter.Add(arg.Item1, arg.Item2);
                    }

                });

                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<T>(procedureName, parameter, commandTimeout: timeout, commandType: CommandType.StoredProcedure);
                    return result?.ToList() ?? new List<T>();
                }
            }
            catch (Exception ex)
            {
                return new List<T>();
            }
        }

        public async Task<T> GetSingle<T>(string connectionString, string procedureName, List<Tuple<string, string>> arguments, int timeout = 300)
        {
            try
            {
                DynamicParameters parameter = new DynamicParameters();
                arguments?.ForEach(arg =>
                {
                    if (arg.Item2 == "-=-" || string.IsNullOrWhiteSpace(arg.Item2))
                    {
                        parameter.Add(arg.Item1, null);
                    }
                    else
                    {
                        parameter.Add(arg.Item1, arg.Item2);
                    }

                });
                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<T>(procedureName, parameter, commandTimeout: timeout, commandType: CommandType.StoredProcedure);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public IDbConnection CreateConnection(string connectionString)
        {
           var connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
