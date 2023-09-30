using Microsoft.EntityFrameworkCore;

namespace Z.MyToDoWebAPI.Context
{
    public class MyToDoContext : DbContext
    {
        public DbSet<ToDo> ToDo { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Memo> Memo { get; set; }

        public MyToDoContext(DbContextOptions options) : base(options)
        {
        }
    }
}
