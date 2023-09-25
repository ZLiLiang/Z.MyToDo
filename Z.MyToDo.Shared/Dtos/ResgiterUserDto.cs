using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.MyToDo.Shared.Dtos
{
    public class ResgiterUserDto : BaseDto
    {
        private string userName;
        private string account;
        private string passWord;
        private string newpassWord;

        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
        }

        public string Account
        {
            get { return account; }
            set { account = value; OnPropertyChanged(); }
        }

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; OnPropertyChanged(); }
        }

        public string NewPassWord
        {
            get { return newpassWord; }
            set { newpassWord = value; OnPropertyChanged(); }
        }
    }
}
