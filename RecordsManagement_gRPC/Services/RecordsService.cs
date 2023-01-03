using Grpc.Core;
using System.Data.SqlClient;
using System.Linq.Expressions;

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
                    try
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

                                await responseStream.WriteAsync(model);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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

        public override Task<responseModel> AddRecord(NewRecord request, ServerCallContext context)
        {
            responseModel response = new responseModel();

            using (SqlConnection connection = RecordsDbConntectionService.GetConnection())
            {
                string sql = $"INSERT INTO [dbo].[Record] (Performer, Title, Price, StockCount) VALUES " +
                    $"(@performer, @title, @price, @stockCount)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        command.Parameters.Add("@performer", System.Data.SqlDbType.NVarChar).Value = request.Performer;
                        command.Parameters.Add("@title", System.Data.SqlDbType.NVarChar).Value = request.Title;
                        command.Parameters.Add("@price", System.Data.SqlDbType.Float).Value = request.Price;
                        command.Parameters.Add("@stockCount", System.Data.SqlDbType.Int).Value = request.StockCount;

                        int affectedRows = command.ExecuteNonQuery();
                        if (affectedRows > 0)
                        {
                            response.Error = 0;
                            response.Message = "Successfully inserted new record!";
                        }
                        else
                        {
                            response.Error = 1;
                            response.Message = "Insert of new record failed!";
                        }

                        return Task.FromResult(response);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        response.Error = 1;
                        response.Message = "Insert of new record failed!\n" + ex.Message;
                        return Task.FromResult(response);
                    }
                }
            }
        }

        public override Task<responseModel> DeleteRecord(DeleteRecordModel request, ServerCallContext context)
        {
            responseModel response = new responseModel();

            using (SqlConnection connection = RecordsDbConntectionService.GetConnection())
            {

                string sql = $"DELETE FROM [dbo].Record WHERE Id = {request.DeleteRecordId}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        int affectedRows = command.ExecuteNonQuery();
                        if (affectedRows > 0)
                        {
                            response.Error = 0;
                            response.Message = "Successfully deleted the record!";
                        }
                        else
                        {
                            response.Error = 1;
                            response.Message = "Record deletion failed!";
                        }

                        return Task.FromResult(response);
                    }
                    catch(Exception ex)
                    {
                        response.Error = 1;
                        response.Message = "Record deletion failed!\n" + ex.Message;
                        return Task.FromResult(response);
                    }
                }
            }
        }
    }
}
