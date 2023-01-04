using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Net.ClientFactory;
using RecordsManagement_gRPC;
using RecordsManagement_gRPC.Services;

namespace RecordsServiceTest
{
    [TestClass]
    public class AdminAuthServiceTest
    {
        private static GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:8888");

        [TestMethod]
        public void LoginTest()
        {
            ResponseModel expected = new ResponseModel()
            {
                Error = 0,
                Message = "Successfully logged in!"
            };

            var testClient = new AuthenticationService.AuthenticationServiceClient(channel);

            ResponseModel actual = testClient.Login(new AdminModel()
            {
                AdminName = "test",
                AdminPass = "test_0",
            });

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LoginTestWithFail()
        {
            ResponseModel expected = new ResponseModel()
            {
                Error = 1,
                Message = "Failed login attempt!"
            };

            var testClient = new AuthenticationService.AuthenticationServiceClient(channel);

            ResponseModel actual = testClient.Login(new AdminModel()
            {
                AdminName = "admin",
                AdminPass = "no_passwd",
            });

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestUpdateAccount()
        {
            ResponseModel expected = new ResponseModel()
            {
                Error = 1,
                Message = "Failed login attempt!"
            };

            var testClient = new AuthenticationService.AuthenticationServiceClient(channel);

            ResponseModel actual = testClient.ChangeAccDetails(new UpdateAdminModel()
            {
                //these should be changed according to the state in the db
                CurrAdminName = "test",
                CurrAdminPass = "test_0",
                NewAdminName = "testy",
                NewAdminPass = "test_0_0"
            });
        }
    }
}
