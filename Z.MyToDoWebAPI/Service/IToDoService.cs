using Z.MyToDo.Shared.Dtos;
using Z.MyToDo.Shared.Parameters;

namespace Z.MyToDoWebAPI.Service
{
    public interface IToDoService : IBaseService<ToDoDto>
    {
        Task<ApiResponse> GetAllAsync(ToDoParameter query);

        Task<ApiResponse> Summary();
    }
}
