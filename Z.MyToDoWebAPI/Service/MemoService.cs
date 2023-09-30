using AutoMapper;
using Z.MyToDo.Shared.Dtos;
using Z.MyToDo.Shared.Parameters;
using Z.MyToDoWebAPI.Context;
using Z.MyToDoWebAPI.Context.UnitOfWork;

namespace Z.MyToDoWebAPI.Service
{
    /// <summary>
    /// 备忘录的实现
    /// </summary>
    public class MemoService : IMemoService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public MemoService(IUnitOfWork work, IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }

        public async Task<ApiResponse> AddAsync(MemoDto model)
        {
            try
            {
                var memo = mapper.Map<Memo>(model);
                await work.GetRepository<Memo>().InsertAsync(memo);
                if (await work.SaveChangesAsync() > 0)
                {
                    return new ApiResponse(true, memo);
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
                var repository = work.GetRepository<Memo>();
                var memo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                repository.Delete(memo);
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

        public async Task<ApiResponse> GetAllAsync(QueryParameter query)
        {
            try
            {
                var repository = work.GetRepository<Memo>();
                var memos = await repository.GetPagedListAsync(predicate: x => string.IsNullOrWhiteSpace(query.Search) ? true : x.Title.Contains(query.Search),
                    pageIndex: query.PageIndex,
                    pageSize: query.PageSize,
                    orderBy: source => source.OrderByDescending(t => t.CreateDate));
                return new ApiResponse(true, memos);
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
                var repository = work.GetRepository<Memo>();
                var memo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(id));
                return new ApiResponse(true, memo);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(MemoDto model)
        {
            try
            {
                var dbMemo = mapper.Map<Memo>(model);
                var repository = work.GetRepository<Memo>();
                var memo = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id.Equals(dbMemo.Id));

                memo.Title = dbMemo.Title;
                memo.Content = dbMemo.Content;
                memo.UpdateDate = DateTime.Now;

                repository.Update(memo);

                if (await work.SaveChangesAsync() > 0)
                {
                    return new ApiResponse(true, memo);
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
