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
    public interface IToDoService : IBaseService<ToDoDto>
    {
        Task<ApiResponse<PagedList<ToDoDto>>> GetAllFilterAsync(ToDoParameter parameter);

        Task<ApiResponse<SummaryDto>> SummaryAsync();
    }
}
