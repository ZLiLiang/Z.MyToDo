using Z.MyToDo.Shared.Dtos;

namespace Z.MyToDoWebAPI.Service
{
    public interface ILoginService
    {
        Task<ApiResponse> LoginAsync(string Account, string Password);

        Task<ApiResponse> Resgiter(UserDto user);
    }
}
