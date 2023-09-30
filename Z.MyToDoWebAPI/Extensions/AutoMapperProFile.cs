using AutoMapper;
using Z.MyToDo.Shared.Dtos;
using Z.MyToDoWebAPI.Context;

namespace Z.MyToDoWebAPI.Extensions
{
    public class AutoMapperProFile : MapperConfigurationExpression
    {
        public AutoMapperProFile()
        {
            CreateMap<ToDo, ToDoDto>().ReverseMap();
            CreateMap<Memo, MemoDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
