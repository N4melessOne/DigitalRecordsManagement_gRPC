using Grpc.Core;
using System.Data.SqlClient;

namespace RecordsManagement_gRPC.Services
{
    public class AdminAuthService : AuthenticationService.AuthenticationServiceBase
    {
        private readonly ILogger<RecordsService> logger;

        public AdminAuthService(ILogger<RecordsService> logger)
        {
            this.logger = logger;
        }

        public override Task<ResponseModel> Login(AdminModel request, ServerCallContext context)
        {
            ResponseModel response = new ResponseModel();

            using (SqlConnection connection = RecordsDbConntectionService.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM [dbo].Admins WHERE AdminName = @adminName AND AdminPass = @adminPass";
                using (SqlCommand command = new SqlCommand(sql, connection)) 
                {
                    try
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@adminName", request.AdminName);
                        command.Parameters.AddWithValue("@adminPass", request.AdminPass);

                        int returnedRows = (int)command.ExecuteScalar();
                        if (returnedRows > 0)
                        {
                            response.Error = 0;
                            response.Message = "Successfully logged in!";
                            return Task.FromResult(response);
                        }
                        else
                        {
                            response.Error = 1;
                            response.Message = "Failed login attempt!";
                            return Task.FromResult(response);
                        }
                    }
                    catch (Exception ex)
                    {
                        response.Error = 1;
                        response.Message = "There were some problems during login process.\n" + ex.Message;
                        return Task.FromResult(response);
                    }
                }
            }
        }


        //KNOWN ISSUE:
        //Can be dangerous to set the sql string up that way becaues of the ',' characters at the end!
        //FIXED IT by a lot of if-else if cases.
        public override Task<ResponseModel> ChangeAccDetails(UpdateAdminModel request, ServerCallContext context)
        {
            ResponseModel response = new ResponseModel();
            
            if (request.HasNewAdminName || request.HasNewAdminPass)
            {
                using (SqlConnection connection = RecordsDbConntectionService.GetConnection())
                {
                    if (GetAdminId(request.CurrAdminName, request.CurrAdminPass, connection) > 0)
                    {
                        try
                        {
                            string sql = "UPDATE [dbo].Admins SET ";
                            if (request.HasNewAdminName && request.HasNewAdminPass)
                                sql += "AdminName = @adminName, ";
                            else if (request.HasNewAdminName)
                                sql += "AdminName = @adminName ";
                            if (request.HasNewAdminPass)
                                sql += "AdminPass = @adminPass ";
                            sql += $"WHERE AdminName = {request.CurrAdminName} AND AdminPass = {request.CurrAdminPass}";
                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                connection.Open();
                                int affectedRows = command.ExecuteNonQuery();
                                if (affectedRows > 0)
                                {
                                    response.Error = 0;
                                    response.Message = "Updated account details successfully!";
                                    return Task.FromResult(response);
                                }
                                else
                                {
                                    response.Error = 1;
                                    response.Message = "Failed to update account details!";
                                    return Task.FromResult(response);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            response.Error = 1;
                            response.Message = "There were no changes made!\n" + ex.Message;
                            return Task.FromResult(response);
                        }
                    }
                    else
                    {
                        response.Error = 1;
                        response.Message = "There is no admin with the given creditentials!";
                        return Task.FromResult(response);
                    }
                }
            }
            else
            {
                response.Error = 1;
                response.Message = "There were no changes made!";
                return Task.FromResult(response);
            }
        }


        private int GetAdminId(string adminName, string adminPass, SqlConnection connection) 
        {
            string sql = "SELECT Id FROM [dbo].Admins WHERE AdminName = @adminName AND AdminPass = @adminPass";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@adminName", adminName);
                command.Parameters.AddWithValue("@adminPass", adminPass);
                int adminId = (int)command.ExecuteScalar();

                if (adminId > 0)
                {
                    connection.Close();
                    return adminId;
                }
                else
                {
                    connection.Close();
                    return 0;
                }
            }
        }
        
    }
}
