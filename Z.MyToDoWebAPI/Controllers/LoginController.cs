using Microsoft.AspNetCore.Mvc;
using Z.MyToDo.Shared.Dtos;
using Z.MyToDoWebAPI.Service;

namespace Z.MyToDoWebAPI.Controllers
{
    /// <summary>
    /// 账户控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService service;

        public LoginController(ILoginService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<ApiResponse> Login([FromBody] UserDto param) => await service.LoginAsync(param.Account, param.PassWord);

        [HttpPost]
        public async Task<ApiResponse> Resgiter([FromForm] UserDto param) => await service.Resgiter(param);
    }
}
