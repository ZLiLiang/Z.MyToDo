using Microsoft.EntityFrameworkCore;
using Z.MyToDoWebAPI.Context.UnitOfWork;

namespace Z.MyToDoWebAPI.Context.Repository
{
    public class UserRepository : Repository<User>, IRepository<User>
    {
        public UserRepository(MyToDoContext dbContext) : base(dbContext)
        {
        }
    }
}
