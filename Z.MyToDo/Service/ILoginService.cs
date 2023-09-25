using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.MyToDo.Shared;
using Z.MyToDo.Shared.Dtos;

namespace Z.MyToDo.Service
{
    public interface ILoginService
    {
        Task<ApiResponse> Login(UserDto user);

        Task<ApiResponse> Resgiter(UserDto user);
    }
}
