using Contracts.Responses;
using System.Linq.Expressions;

namespace Contracts.interfaces.Repository
{
    public interface IRepositoryBase<T, TDto, TCreateDto, TUpdateDto>
    {
        Task<ParentResponseModel> FindById(int id);
        Task<ParentResponseModel> FindAll();
        Task<ParentResponseModel> FindByCondition(Expression<Func<T, bool>> expression);
        Task<ParentResponseModel> FindAllPaged(int page = 1, int pagesize = 10);
        Task<ParentResponseModel> FindByConditionPaged(Expression<Func<T, bool>> expression, int page = 1, int pagesize = 10);
        Task<ParentResponseModel> Create(TCreateDto entityCreate);
        Task<ParentResponseModel> Update(TUpdateDto entityUpdate);
        Task<ParentResponseModel> Delete(int id, bool softDelete = true);
    }
}
