using System.Data.SqlClient;

namespace SystemIntegration_2018
{
    class ConnectionManager
    {
        private static string dataSource = "Data Source=172.17.0.2, 1433 ;Initial Catalog=SystemIntegrationDB;Connection Timeout=45;";
        private static string user = "User ID=SA;Password=Password1;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(dataSource + user);
        }
    }
}
