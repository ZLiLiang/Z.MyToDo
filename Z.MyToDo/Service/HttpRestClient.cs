using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;
using Z.MyToDo.Shared;

namespace Z.MyToDo.Service
{
    public class HttpRestClient
    {
        //private readonly string apiUrl;
        protected readonly RestClient client;

        public HttpRestClient(string apiUrl)
        {
            //this.apiUrl = apiUrl;
            client = new RestClient(apiUrl);
        }

        public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
        {
            var request = new RestRequest(baseRequest.Route, baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);

            if (baseRequest.Parameter != null)
            {
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
            }

            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
            }
            else
            {
                return new ApiResponse
                {
                    Status = false,
                    Result = null,
                    Message = response.ErrorMessage
                };
            }
        }

        public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        {
            var request = new RestRequest(baseRequest.Route, baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);

            if (baseRequest.Parameter != null)
            {
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
            }

            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
            }
            else
            {
                return new ApiResponse<T>
                {
                    Status = false,
                    Message = response.ErrorMessage
                };
            }
        }
    }
}
