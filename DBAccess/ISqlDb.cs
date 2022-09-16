using Microsoft.Data.SqlClient;
using System.Data;

namespace DBAccess
{
    public interface ISqlDb
    {
        Task<DataTable> AsyncExecuteProcedureReturnDataTable(string connectionStringName, string procName, params SqlParameter[] parameters);
        DataSet ExecuteProcedureReturnDataSet(string connectionStringName, string procName, params SqlParameter[] parameters);
        DataTable ExecuteProcedureReturnDataTable(string connectionStringName, string procName, params SqlParameter[] parameters);
        List<SqlParameter> ExecuteProcedureReturnOutputParameters(string connectionStringName, string procName, params SqlParameter[] parameters);
    }
}