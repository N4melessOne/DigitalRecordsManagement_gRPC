using Grpc.Net.Client;
using RecordsManagement_gRPC;
using System.Data.SqlClient;

namespace RecordsServiceTest
{
    [TestClass]
    public class RecordsServiceTest
    {
        private static GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:8888");

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

            var testClient = new RecordsManagementService.RecordsManagementServiceClient(channel);

            recordModel actual = testClient.GetRecordById(new IdOfRecord { RecordId = 3 });

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task TestAllRecords()
        {
            List<recordModel> expected = new List<recordModel>();
            using (SqlConnection connection = RecordsDbConntectionService.GetConnection())
            {
                string sql = "SELECT * FROM [dbo].Record";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            recordModel model = new recordModel();
                            model.RecordId = reader.GetInt32(0);
                            model.Performer = reader.GetString(1);
                            model.Title = reader.GetString(2);
                            model.Price = reader.GetDouble(3);
                            model.StockCount = reader.GetInt32(4);
                            expected.Add(model);
                        }
                    }
                }
            }

            List<recordModel> actual = new List<recordModel>();

            var testClient = new RecordsManagementService.RecordsManagementServiceClient(channel);

            using (var call = testClient.AllRecords(new allRecordsRequest()))
            {
                while (await call.ResponseStream.MoveNext(new CancellationToken()))
                {
                    actual.Add(call.ResponseStream.Current);
                }
            }

            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void TestNewRecord()
        {
            responseModel expected = new responseModel
            {
                Error = 0,
                Message = "Successfully inserted new record!"
            };

            var testClient = new RecordsManagementService.RecordsManagementServiceClient(channel);

            NewRecord newRecord = new NewRecord()
            {
                Performer = "Like Moths To Flames",
                Title = "No Eternety in Gold",
                Price = 12,
                StockCount = 6
            };
            responseModel actual = testClient.AddRecord(newRecord);

            Assert.AreEqual(expected, actual);
        }
    }
}