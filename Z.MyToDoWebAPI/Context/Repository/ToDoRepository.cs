using Microsoft.EntityFrameworkCore;
using Z.MyToDoWebAPI.Context.UnitOfWork;

namespace Z.MyToDoWebAPI.Context.Repository
{
    public class ToDoRepository : Repository<ToDo>, IRepository<ToDo>
    {
        public ToDoRepository(MyToDoContext dbContext) : base(dbContext)
        {
        }
    }
}
