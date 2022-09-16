using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAccess
{
    public class SqlDb : ISqlDb
    {
        private readonly IConfiguration _config;

        public SqlDb(IConfiguration config)
        {
            _config = config;
        }
        public DataSet ExecuteProcedureReturnDataSet(string connectionStringName, string procName, params SqlParameter[] parameters)
        {
            DataSet? result = null;
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                using (var command = sqlConnection.CreateCommand())
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(command))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = procName;
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        result = new DataSet();
                        sda.Fill(result);
                    }
                }
            }
            return result;
        }

        public List<SqlParameter> ExecuteProcedureReturnOutputParameters(string connectionStringName, string procName, params SqlParameter[] parameters)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);
            List<SqlParameter> result = new List<SqlParameter>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                using (var command = sqlConnection.CreateCommand())
                {
                    sqlConnection.Open();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = procName;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    command.ExecuteNonQuery();
                    var str = command.CommandText;
                    foreach (var item in parameters)
                    {
                        result.Add(new SqlParameter(item.ParameterName, item.Value));
                    }
                }
            }
            return result;
        }

        public DataTable ExecuteProcedureReturnDataTable(string connectionStringName, string procName, params SqlParameter[] parameters)
        {
            DataTable result = new DataTable();
            string connectionString = _config.GetConnectionString(connectionStringName);
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = procName;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        result.Load(reader);
                        reader.Close();
                    }
                }
            }
            return result;
        }
        public async Task<DataTable> AsyncExecuteProcedureReturnDataTable(string connectionStringName, string procName, params SqlParameter[] parameters)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            DataTable result = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = procName;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        result.Load(reader);
                        reader.Close();
                    }
                }
            }
            return result;
        }
    }
}
