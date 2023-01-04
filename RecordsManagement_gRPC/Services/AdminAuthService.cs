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
    }
}
