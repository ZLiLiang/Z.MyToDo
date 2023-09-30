
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Z.MyToDoWebAPI.Context;
using Z.MyToDoWebAPI.Context.Repository;
using Z.MyToDoWebAPI.Context.UnitOfWork;
using Z.MyToDoWebAPI.Extensions;
using Z.MyToDoWebAPI.Service;

namespace Z.MyToDoWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MyToDo.Api",
                    Version = "v1",
                });
            });

            var connectionString = builder.Configuration.GetConnectionString("ToDoConnection");
            builder.Services.AddDbContext<MyToDoContext>(option =>
            {
                option.UseSqlite(connectionString);
            }).AddUnitOfWork<MyToDoContext>()
            .AddCustomRepository<ToDo, ToDoRepository>() //AddScoped<IRepository<ToDo>, ToDoRepository>()
            .AddCustomRepository<Memo, MemoRepository>()
            .AddCustomRepository<User, UserRepository>();

            builder.Services.AddTransient<IToDoService, ToDoService>();
            builder.Services.AddTransient<IMemoService, MemoService>();
            builder.Services.AddTransient<ILoginService, LoginService>();

            //Ìí¼ÓAutoMapper
            builder.Services.AddSingleton(new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProFile());
            }).CreateMapper());


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyToDo.Api v1"));
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}