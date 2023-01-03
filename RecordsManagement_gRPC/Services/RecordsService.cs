using Grpc.Core;
using System.Data.SqlClient;

namespace RecordsManagement_gRPC.Services
{
    public class RecordsService : RecordsManagementService.RecordsManagementServiceBase
    {
        private readonly ILogger<RecordsService> logger;

        public RecordsService(ILogger<RecordsService> logger)
        {
            this.logger = logger;
        }

        public override async Task AllRecords(allRecordsRequest request, 
            IServerStreamWriter<recordModel> responseStream, ServerCallContext context)
        {
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
                            model.Price = reader.GetFloat(3);
                            model.StockCount = reader.GetInt32(4);

                            await responseStream.WriteAsync(model);
                        }
                    }
                }
            }
        }

        public override Task<recordModel> GetRecordById(IdOfRecord request, ServerCallContext context)
        {
            using (SqlConnection connection = RecordsDbConntectionService.GetConnection())
            {
                string sql = $"SELECT * FROM [dbo].Record WHERE Id = {request.RecordId}";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        bool result = reader.Read();
                        if (result)
                        {
                            recordModel model = new recordModel();
                            model.RecordId = reader.GetInt32(0);
                            model.Performer = reader.GetString(1);
                            model.Title = reader.GetString(2);
                            model.Price = reader.GetDouble(3);
                            model.StockCount = reader.GetInt32(4);

                            return Task.FromResult(model);
                        }
                        else
                            return null!;
                    }
                }
            }
        }
    }
}
