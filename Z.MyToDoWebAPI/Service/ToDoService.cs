using AutoMapper;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Reflection.Metadata;
using Z.MyToDo.Shared.Dtos;
using Z.MyToDo.Shared.Parameters;
using Z.MyToDoWebAPI.Context;
using Z.MyToDoWebAPI.Context.UnitOfWork;

namespace Z.MyToDoWebAPI.Service
{
    /// <summary>
    /// 待办事项的实现
    /// </summary>
    public class ToDoService : IToDoService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public ToDoService(IUnitOfWork work, IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(ToDoDto model)
        {
            try
            {
                var todo = mapper.Map<ToDo>(model);
                await work.GetRepository<ToDo>().InsertAsync(todo);
                if (await work.SaveChangesAsync() > 0)
                {
                    return new ApiResponse(true, todo);
                }
                return new ApiResponse("添加数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                repository.Delete(todo);
                if (await work.SaveChangesAsync() > 0)
                {
                    return new ApiResponse(true, "");
                }
                return new ApiResponse("删除数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync(ToDoParameter query)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todos = await repository.GetPagedListAsync(
                   predicate: x => (string.IsNullOrWhiteSpace(query.Search) ? true : x.Title.Contains(query.Search))
                && (query.Status == null ? true : x.Status.Equals(query.Status)),
                   pageIndex: query.PageIndex,
                   pageSize: query.PageSize,
                   orderBy: source => source.OrderByDescending(t => t.CreateDate));
                return new ApiResponse(true, todos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter query)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todos = await repository.GetPagedListAsync(
                   predicate: x => string.IsNullOrWhiteSpace(query.Search) ? true : x.Title.Contains(query.Search),
                   pageIndex: query.PageIndex,
                   pageSize: query.PageSize,
                   orderBy: source => source.OrderByDescending(t => t.CreateDate));
                return new ApiResponse(true, todos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, todo);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> Summary()
        {
            try
            {
                //待办事项结果
                var todos = await work.GetRepository<ToDo>().GetAllAsync(orderBy: source => source.OrderByDescending(t => t.CreateDate));

                //备忘录结果
                var memos = await work.GetRepository<Memo>().GetAllAsync(orderBy: source => source.OrderByDescending(t => t.CreateDate));

                SummaryDto summary = new SummaryDto();
                summary.Sum = todos.Count(); //汇总待办事项数量
                summary.CompletedCount = todos.Where(t => t.Status == 1).Count(); //统计完成数量
                summary.CompletedRatio = (summary.CompletedCount / (double)summary.Sum).ToString("0%"); //统计完成率
                summary.MemoeCount = memos.Count();  //汇总备忘录数量
                summary.ToDoList = new ObservableCollection<ToDoDto>(mapper.Map<List<ToDoDto>>(todos.Where(t => t.Status == 0)));
                summary.MemoList = new ObservableCollection<MemoDto>(mapper.Map<List<MemoDto>>(memos));

                return new ApiResponse(true, summary);
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, "");
            }
        }

        public async Task<ApiResponse> UpdateAsync(ToDoDto model)
        {
            try
            {
                var dbToDo = mapper.Map<ToDo>(model);
                var repository = work.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbToDo.Id));

                todo.Title = dbToDo.Title;
                todo.Content = dbToDo.Content;
                todo.Status = dbToDo.Status;
                todo.UpdateDate = DateTime.Now;

                repository.Update(todo);

                if (await work.SaveChangesAsync() > 0)
                {
                    return new ApiResponse(true, todo);
                }
                return new ApiResponse("更新数据异常！");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
