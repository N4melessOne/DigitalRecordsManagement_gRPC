using Grpc.Net.Client;
using RecordsManagement_gRPC;
using System.Data.SqlClient;

namespace RecordsServiceTest
{
    [TestClass]
    public class RecordsServiceTest
    {
        [TestMethod]
        public void TestConnection()
        {
            SqlConnection sqlConnection = null!;
            sqlConnection = RecordsDbConntectionService.GetConnection();

            Assert.IsNotNull(sqlConnection);
        }


        [TestMethod]
        public void TestRecordById()
        {
            recordModel expected = new recordModel
            {
                RecordId = 3,
                Performer = "Architects",
                Title = "Holy Hell",
                Price = 11.99,
                StockCount = 18
            };

            var channel = GrpcChannel.ForAddress("https://localhost:8888");
            var testClient = new RecordsManagementService.RecordsManagementServiceClient(channel);

            recordModel actual = testClient.GetRecordById(new IdOfRecord { RecordId = 3 });

            Assert.AreEqual(expected, actual);
        }
    }
}