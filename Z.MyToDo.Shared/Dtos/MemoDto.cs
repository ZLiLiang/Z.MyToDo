using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.MyToDo.Shared.Dtos
{
    /// <summary>
    /// 备忘录数据实体
    /// </summary>
    public class MemoDto : BaseDto
    {
        private string title;
        private string content;
        public int Status { get; set; }

        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }

        public string Content
        {
            get { return content; }
            set { content = value; OnPropertyChanged(); }
        }
    }
}
