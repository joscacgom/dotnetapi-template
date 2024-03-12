using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Data
{
    public class DataContextDapper
    {
        private readonly IConfiguration _config;
        public DataContextDapper(IConfiguration config)
        {
            _config = config;
        }
       
       public IEnumerable<T> LoadData<T>(string sql)
        {
            string? connectionString = _config.GetConnectionString("DefaultConnection");
            IDbConnection dbConnection = new SqlConnection(connectionString);

            return dbConnection.Query<T>(sql);
        }

        public T? LoadDataSingle<T>(string sql)
        {
            string? connectionString = _config.GetConnectionString("DefaultConnection");
            IDbConnection dbConnection = new SqlConnection(connectionString);

            return dbConnection.QuerySingleOrDefault<T>(sql);
        }

        public T? LoadDataSingleWithEntity<T>(string sql, T entity)
        {
            string? connectionString = _config.GetConnectionString("DefaultConnection");
            IDbConnection dbConnection = new SqlConnection(connectionString);

            return dbConnection.QuerySingleOrDefault<T>(sql, entity);
        }

        public bool ExecuteData<T>(string sql, T data)
        {
            string? connectionString = _config.GetConnectionString("DefaultConnection");
            IDbConnection dbConnection = new SqlConnection(connectionString);

            return dbConnection.Execute(sql, data) > 0;
        }

        public bool ExecuteWithParameters(string sql, List<SqlParameter> parameters)
        {
            string? connectionString = _config.GetConnectionString("DefaultConnection");
            IDbConnection dbConnection = new SqlConnection(connectionString);

            SqlCommand commandWithParameters = new SqlCommand(sql);

            foreach(SqlParameter parameter in parameters)
            {
                commandWithParameters.Parameters.Add(parameter);
            }

            dbConnection.Open();

            commandWithParameters.Connection = (SqlConnection)dbConnection;
            
            return commandWithParameters.ExecuteNonQuery() > 0;
        }
    }
}