using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.MyToDo.Shared;
using Z.MyToDo.Shared.Dtos;
using Z.MyToDo.Shared.Parameters;

namespace Z.MyToDo.Service
{
    public class ToDoService : BaseService<ToDoDto>, IToDoService
    {
        private readonly HttpRestClient client;

        public ToDoService(HttpRestClient client, string serviceName) : base(client, "ToDo")
        {
            this.client = client;
        }

        public async Task<ApiResponse<PagedList<ToDoDto>>> GetAllFilterAsync(ToDoParameter parameter)
        {
            BaseRequest request = new BaseRequest
            {
                Method = RestSharp.Method.Get,
                Route = $"api/ToDo/GwtAll?pageIndex={parameter.PageIndex}" +
                $"&pageSize={parameter.PageSize}" +
                $"&search={parameter.Search}" +
                $"&status={parameter.Status}"
            };
            return await client.ExecuteAsync<PagedList<ToDoDto>>(request);
        }

        public async Task<ApiResponse<SummaryDto>> SummaryAsync()
        {
            BaseRequest request = new BaseRequest
            {
                Route = "api/ToDo/Summary"
            };
            return await client.ExecuteAsync<SummaryDto>(request);
        }
    }
}
