using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Z.MyToDoWebAPI.Context;

namespace Z.MyToDoWebAPI
{
    public class DbContextDesignTimeFactory : IDesignTimeDbContextFactory<MyToDoContext>
    {
        public MyToDoContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<MyToDoContext> builder = new DbContextOptionsBuilder<MyToDoContext>();
            builder.UseSqlite("Data Source=to.db");
            return new MyToDoContext(builder.Options);
        }
    }
}
