using Domain.Common;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public interface IReadOnlyRepo<TEntity, TDto> where TEntity : class, IEntityModel
        where TDto : IDto, new()
    {
        Task<TDto> GeByIdAsync(int id);
        Task<PagedResult<TDto>> GetAllPagedAsync(int page, int pageSize);
        Task<IEnumerable<TDto>> GetAllAsync();
    }
}