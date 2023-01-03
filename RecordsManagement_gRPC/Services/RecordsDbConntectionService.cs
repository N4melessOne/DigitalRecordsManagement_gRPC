using System;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace RecordsManagement_gRPC.Services
{
    public class RecordsDbConntectionService
    {
        private static readonly string connectionString =  @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog = RecordsManagementDb; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }
    }
}
