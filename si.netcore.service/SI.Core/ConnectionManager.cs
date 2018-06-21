using System.Data.SqlClient;

namespace SystemIntegration_2018
{
    class ConnectionManager
    {
        private static string dataSource = "Data Source=LAPTOP-AH4RCK16;Initial Catalog=SystemIntegrationDB;Connection Timeout=45;";
        private static string user = "User ID=admin;Password=admin;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(dataSource + user);
        }
    }
}
