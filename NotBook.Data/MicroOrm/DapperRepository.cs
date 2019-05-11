using System;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace NotBook.Data.MicroOrm
{
    public class DapperRepository : IMicroOrmRepository, IDisposable
    {
        private readonly IDbConnection _connection;
        
        public DapperRepository(IConfiguration configuration, IHostingEnvironment environment)
        {
            var connectionString = configuration.GetConnectionString("MySQLProductionConnection");
            if (environment.IsDevelopment())
            {
                connectionString = configuration.GetConnectionString("MySQLDevelopmentConnection");
            }
            
            _connection = new MySqlConnection(connectionString);
        }

        public IDbConnection Connection
        {
            get
            {
                if (_connection.State == ConnectionState.Broken) 
                    _connection.Close();

                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();

                return _connection;
            }
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}