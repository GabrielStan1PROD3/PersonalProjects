using Domain.Common;
using Domain.Interfaces;
using Infrastructure.DbConnection;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Infrastructure.Repositories
{
    public class ReadOnlyRepo<TEntity, TDto> : EfGenericRepo<TEntity, TDto>, IReadOnlyRepo<TEntity,TDto>
     where TEntity : class, IEntityModel
     where TDto : IDto, new()

    {

        public ReadOnlyRepo(CookingAppContext context) : base(context) { }


        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await _dbSet.ToListAsync();
            return entities.Select(CastToDto).ToList();
        }
    



        public async Task<IEnumerable<TOwnedDto>> GetOwnedDtosAsync<TOwnedDto>() 
        {
            List<TOwnedDto> tdos = new List<TOwnedDto>();


            Type? ownedEntityType=  _getOwnedEntityType<TOwnedDto>((new Regex("Dto")));
            var entities = _dbSet.AsQueryable();
            Func<TEntity, TOwnedDto> ownedDelegate = _getOrAddDelegateToCache<TOwnedDto>();
          
            
            foreach (var entity in entities)
            {
                tdos.Add(ownedDelegate(entity));
            }
             return tdos;
        }

        public async Task<PagedResult<TDto>> GetAllPagedAsync(int page = 1, int pageSize = 10)
        {
            var query = _dbSet.AsQueryable();
            var totalItems = await query.CountAsync();

            var items = await query
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return new PagedResult<TDto>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                Items = items.Select(CastToDto).ToList()
            };
        }
        public async Task<TDto> GeByIdAsync(int id)
        {
            var entity = await _dbSet.SingleOrDefaultAsync(e => e.Id == id)
                ?? throw new DbNotFoundException("We didnt find any item with this id");
            var dto = CastToDto(entity);
            return dto;
        }

    }
}
