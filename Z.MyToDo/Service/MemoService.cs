using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.MyToDo.Shared.Dtos;

namespace Z.MyToDo.Service
{
    public class MemoService : BaseService<MemoDto>, IMemoService
    {
        public MemoService(HttpRestClient client, string serviceName) : base(client, "Memo")
        {
        }
    }
}
