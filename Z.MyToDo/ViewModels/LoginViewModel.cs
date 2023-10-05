using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.MyToDo.Extensions;
using Z.MyToDo.Service;
using Z.MyToDo.Shared.Dtos;

namespace Z.MyToDo.ViewModels
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        #region 属性

        private readonly ILoginService loginService;
        private readonly IEventAggregator aggregator;
        public event Action<IDialogResult> RequestClose;

        public string Title { get; set; } = "ToDo";

        public DelegateCommand<string> ExecuteCommand { get; private set; }

        private int selectIndex;

        public int SelectIndex
        {
            get { return selectIndex; }
            set { selectIndex = value; RaisePropertyChanged(); }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        private string passWord;

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; RaisePropertyChanged(); }
        }

        private ResgiterUserDto userDto;

        public ResgiterUserDto UserDto
        {
            get { return userDto; }
            set { userDto = value; RaisePropertyChanged(); }
        }

        #endregion

        public LoginViewModel(ILoginService loginService, IEventAggregator aggregator)
        {
            this.loginService = loginService;
            this.aggregator = aggregator;
            UserDto = new ResgiterUserDto();
            ExecuteCommand = new DelegateCommand<string>(Execute);
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            LoginOut();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }

        private void Execute(string function)
        {
            switch (function)
            {
                case "Login": Login(); break;
                case "LoginOut": LoginOut(); break;
                case "Resgiter": Resgiter(); break;
                case "ResgiterPage": SelectIndex = 1; break;
                case "Return": SelectIndex = 0; break;
            }
        }

        /// <summary>
        /// 登陆
        /// </summary>
        private async void Login()
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(PassWord))
            {
                return;
            }

            var loginResult = await loginService.Login(new UserDto
            {
                Account = UserName,
                PassWord = PassWord,
            });

            if (loginResult != null && loginResult.Status)
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            else
            {
                //登录失败提示...
                aggregator.SendMessage(loginResult.Message, "Login");
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        private async void Resgiter()
        {
            if (string.IsNullOrWhiteSpace(UserDto.Account) ||
                string.IsNullOrWhiteSpace(UserDto.UserName) ||
                string.IsNullOrWhiteSpace(UserDto.PassWord) ||
                string.IsNullOrWhiteSpace(UserDto.NewPassWord))
            {
                aggregator.SendMessage("请输入完整的注册信息！", "Login");
                return;
            }

            if (UserDto.PassWord != UserDto.NewPassWord)
            {
                aggregator.SendMessage("密码不一致,请重新输入！", "Login");
            }

            var resgiterResult = await loginService.Resgiter(new UserDto
            {
                Account = UserDto.Account,
                UserName = UserDto.UserName,
                PassWord = UserDto.PassWord,
            });

            if (resgiterResult != null && resgiterResult.Status)
            {
                aggregator.SendMessage("注册成功", "Login");
                //注册成功,返回登录页页面
                SelectIndex = 0;
            }
            else
            {
                aggregator.SendMessage(resgiterResult.Message, "Login");
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        public void LoginOut()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }
    }
}
