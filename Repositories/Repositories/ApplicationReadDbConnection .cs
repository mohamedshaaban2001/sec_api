using Contracts.interfaces.Repository;
using Dapper;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class ApplicationReadDbConnection : IApplicationReadDbConnection, IDisposable
    {
        private readonly string _connectionString;
        private readonly string _databaseType;

        public ApplicationReadDbConnection(string connectionString, string databaseType)
        {
            _connectionString = connectionString;
            _databaseType = databaseType;
        }

        // Creates a new database connection for each query
        private IDbConnection CreateConnection()
        {
            IDbConnection connection = _databaseType switch
            {
                "Postgres" => new NpgsqlConnection(_connectionString),
                "Oracle" => new OracleConnection(_connectionString),
                _ => throw new InvalidOperationException("Unsupported database type.")
            };

            connection.Open(); // Open connection
            return connection;
        }

        // Execute a query and return a list of results
        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            using (var connection = CreateConnection()) // Creates a new connection
            {
                return (await connection.QueryAsync<T>(sql, param, transaction)).AsList();
            }
        }

        // Execute a query and return a single record
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            using (var connection = CreateConnection()) // Creates a new connection
            {
                return await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
            }
        }

        // Execute a query expecting exactly one result
        public async Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            using (var connection = CreateConnection()) // Creates a new connection
            {
                return await connection.QuerySingleAsync<T>(sql, param, transaction);
            }
        }

        public void Dispose()
        {
            // Nothing to dispose because connections are managed per query
        }
    }
}
