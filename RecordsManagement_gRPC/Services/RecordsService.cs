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
                if (IsThereARecordWithId(request.DeleteRecordId, connection))
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
                        catch (Exception ex)
                        {
                            response.Error = 1;
                            response.Message = "Record deletion failed!\n" + ex.Message;
                            return Task.FromResult(response);
                        }
                    }
                }
                else
                {
                    response.Error = 1;
                    response.Message = "There is no record in the database with the given id!";
                    return Task.FromResult(response);
                }
            }
        }


        //KNOWN ISSUE:
        //Can be dangerous to set the sql string up that way becaues of the ',' characters at the end!
        //have to figure out a way to do it.
        public override Task<responseModel> UpdateRecord(UpdateRecordModel request, ServerCallContext context)
        {
            responseModel response = new responseModel();

            using (SqlConnection connection = RecordsDbConntectionService.GetConnection())
            {
                if (IsThereARecordWithId(request.UpdateRecordId, connection))
                {
                    //constructing the sql string if any of the attributes changed
                    if (request.HasPerformer || request.HasTitle || request.HasPrice || request.HasStockCount)
                    {
                        string sql = "UPDATE [dbo].Record SET ";
                        if (request.HasPerformer)
                            sql += "Performer = @performer, ";
                        if (request.HasTitle)
                            sql += "Title = @title, ";
                        if (request.HasPrice)
                            sql += "Price = @price, ";
                        if (request.HasStockCount)
                            sql += "StockCount = @stockCount ";
                        sql += "WHERE Id = @updateRecordId";
                        //because of the validations it can't get through without something to change!

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            try
                            {
                                connection.Open();
                                if (request.HasPerformer)
                                    command.Parameters.AddWithValue("@performer", request.Performer);
                                if (request.HasTitle)
                                    command.Parameters.AddWithValue("@title", request.Title);
                                if (request.HasPrice)
                                    command.Parameters.AddWithValue("@price", request.Price);
                                if (request.HasStockCount)
                                    command.Parameters.AddWithValue("@stockCount", request.StockCount);
                                command.Parameters.AddWithValue("@updateRecordId", request.UpdateRecordId);

                                int affectedRows = command.ExecuteNonQuery();
                                if (affectedRows > 0)
                                {
                                    response.Error = 0;
                                    response.Message = "Record was updated successfully!";
                                    return Task.FromResult(response);
                                }
                                else
                                {
                                    response.Error = 1;
                                    response.Message = "Update of record failed!";
                                    return Task.FromResult(response);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                response.Error = 1;
                                response.Message = "Update of record failed!\n" + ex.Message;
                                return Task.FromResult(response);
                            }
                        }
                    }
                    else
                    {
                        response.Error = 1;
                        response.Message = "There were no changes made to the record!";
                        return Task.FromResult(response);
                    }
                }
                else
                {
                    response.Error = 1;
                    response.Message = "There is no record in the database with the given id!";
                    return Task.FromResult(response);
                }
            }
        }
        private bool IsThereARecordWithId(int id, SqlConnection connection)
        {
            string sql = "SELECT COUNT(*) FROM [dbo].Record WHERE Id = @recordId";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@recordId", id);

                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    connection.Close();
                    return true;
                }
                else
                {
                    connection.Close();
                    return false;
                }
            }
        }
    }
}
