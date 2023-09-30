using Microsoft.EntityFrameworkCore;
using Z.MyToDoWebAPI.Context.UnitOfWork;

namespace Z.MyToDoWebAPI.Context.Repository
{
    public class MemoRepository : Repository<Memo>, IRepository<Memo>
    {
        public MemoRepository(MyToDoContext dbContext) : base(dbContext)
        {
        }
    }
}
