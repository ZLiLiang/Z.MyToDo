using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.MyToDo.Shared;
using Z.MyToDo.Shared.Parameters;

namespace Z.MyToDo.Service
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private readonly HttpRestClient client;
        private readonly string serviceName;

        public BaseService(HttpRestClient client, string serviceName)
        {
            this.client = client;
            this.serviceName = serviceName;
        }

        public async Task<ApiResponse<TEntity>> AddAsync(TEntity entity)
        {
            BaseRequest request = new BaseRequest
            {
                Method = RestSharp.Method.Post,
                Route = $"api/{serviceName}/Add",
                Parameter = entity
            };
            return await client.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            BaseRequest request = new BaseRequest
            {
                Method = RestSharp.Method.Delete,
                Route = $"api/{serviceName}/Delete?id={id}"
            };
            return await client.ExecuteAsync(request);
        }

        public async Task<ApiResponse<PagedList<TEntity>>> GetAllAsync(QueryParameter query)
        {
            BaseRequest request = new BaseRequest
            {
                Method = RestSharp.Method.Get,
                Route = $"api/{serviceName}/GetAll?" +
                $"pageIndex={query.PageIndex}" +
                $"&pageSize={query.PageSize}" +
                $"&search={query.Search}"
            };
            return await client.ExecuteAsync<PagedList<TEntity>>(request);
        }

        public async Task<ApiResponse<TEntity>> GetFirstOfDefaultAsync(int id)
        {
            BaseRequest request = new BaseRequest
            {
                Method = RestSharp.Method.Get,
                Route = $"api/{serviceName}/Get?id={id}"
            };
            return await client.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity)
        {
            BaseRequest request = new BaseRequest
            {
                Method = RestSharp.Method.Post,
                Route = $"api/{serviceName}/Update",
                Parameter = entity
            };
            return await client.ExecuteAsync<TEntity>(request);
        }
    }
}
