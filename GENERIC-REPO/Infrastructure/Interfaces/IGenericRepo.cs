using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Interfaces
{
    public interface IGenericRepo<TEntity,TDto>
        where TDto : IDto , new ()
    {

        Task<TEntity> AddAsync(TDto dto);
        Task UpdateAsync(TDto dto,int id);
        Task DeleteAsync(int id);
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto> GeByIdAsync(int id);
   

    }
}
