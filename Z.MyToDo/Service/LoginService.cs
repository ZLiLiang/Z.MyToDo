using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.MyToDo.Shared;
using Z.MyToDo.Shared.Dtos;

namespace Z.MyToDo.Service
{
    public class LoginService : ILoginService
    {
        private readonly HttpRestClient client;
        private readonly string serviceName = "Login";

        public LoginService(HttpRestClient client)
        {
            this.client = client;
        }

        public async Task<ApiResponse> Login(UserDto user)
        {
            BaseRequest request = new BaseRequest
            {
                Method = RestSharp.Method.Post,
                Route = $"api/{serviceName}/Login",
                Parameter = user
            };
            return await client.ExecuteAsync(request);
        }

        public async Task<ApiResponse> Resgiter(UserDto user)
        {
            BaseRequest request = new BaseRequest
            {
                Method = RestSharp.Method.Get,
                Route = $"api/{serviceName}/Resgiter",
                Parameter = user
            };
            return await client.ExecuteAsync(request);
        }
    }
}
