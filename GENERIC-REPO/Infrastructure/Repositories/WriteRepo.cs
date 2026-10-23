using Domain.Interfaces;
using Infrastructure.DbConnection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Infrastructure.Repositories
{
    public class WriteRepo<TEntity, TDto> : EfGenericRepo<TEntity, TDto>, IWriteRepo<TDto>
        where TEntity : class, IEntityModel
        where TDto : IDto, new()
    {
        public WriteRepo(CookingAppContext context) : base(context)
        {

        }


        public async Task UpdateComplexOwnedTypeAsync<TOwnedDto,TUpdateTargetType>(int id, TOwnedDto dto)
        {

            var entity = await _dbSet.SingleOrDefaultAsync(x => x.Id == id)
                        ?? throw new DbNotFoundException("We didnt find any item with this id");

                Type ownedEntityType = _getOwnedEntityType<TOwnedDto>(new Regex("Dto$"));

                IList? collection = _getCollectionOwnedType<TOwnedDto>(entity, ownedEntityType);
            
                PropertyInfo dtoKeyProp = _getKeyProperty(typeof(TOwnedDto));

                PropertyInfo entityKeyProp = _getKeyProperty(ownedEntityType); 

                object? dtoKeyValue = dtoKeyProp.GetValue(dto);

            object? element = collection
                .Cast<object>() // <= key concept
                .SingleOrDefault(x =>
                    Equals(entityKeyProp.GetValue(x), dtoKeyValue))
                ?? throw new InvalidOperationException("No matching element found in the collection.");


            MethodInfo? castMethod = _getMethod(typeof(TOwnedDto), ownedEntityType)
                ?? throw new InvalidOperationException("No explicit cast found for DTO → owned type.");
           
            
            var del = _addOrGetDelegate<TOwnedDto,TUpdateTargetType>(castMethod, ownedEntityType);
                
                var updatedElement = del(dto);
                int index = collection.IndexOf(element);

                collection[index] = updatedElement;


            
        }


        public async Task UpdateEntity(TDto dto, int id)
        {
            var toUpdate = await _dbSet
                .SingleOrDefaultAsync(e => e.Id == id) ??
                throw new DbNotFoundException("We didnt find any item with this id");

            var newEntity = CastToEntity(dto);
            newEntity.Id = id;
            var entries = _context.Entry(toUpdate);


            foreach (var prop in entries.Properties)
            {
                if (
                     prop.Metadata.IsShadowProperty() ||
                     prop.Metadata.IsConcurrencyToken ||
                     prop.Metadata.IsKey() ||
                     prop.Metadata.IsForeignKey()
                     )
                {
                    continue;
                }
                var newValue = typeof(TEntity).GetProperty(prop.Metadata.Name)?.GetValue(newEntity);


                if (newValue == null)
                {
                    continue;
                }
                prop.CurrentValue = newValue;

            }
        }


        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.SingleOrDefaultAsync(e => e.Id == id) ??
                        throw new DbNotFoundException("We didnt find any item with this id");
            ;

            _context.Entry(entity).Property("IsDeleted").CurrentValue = true;
          
        }

     

        public async Task UpdateOwnedTypeAsync<TOwnedDto,TReturnType>(int id, TOwnedDto dto)
        {
            var entity = await _dbSet.SingleOrDefaultAsync(x => x.Id == id)
                ?? throw new DbNotFoundException("We didnt find any item with this id");

            var entityType = _context.Model.FindEntityType(entity.GetType());

            IEnumerable<INavigation> ownedNavigations = _getOwnedNavigation(
                entityType, 
                (IEntityType e) => e.IsOwned()
                );


            INavigation? match = ownedNavigations
                .FirstOrDefault(n => n.TargetEntityType.ClrType.Name + "Dto" == typeof(TOwnedDto).Name)
                ?? throw new InvalidOperationException("DTO does not match any owned type.");
           
            
            var ownedClrType = match.TargetEntityType.ClrType;


            var castMethod = _getMethod(typeof(TOwnedDto), ownedClrType)
                ?? throw new InvalidOperationException("No explicit cast found for DTO → owned type.");
           
            
          var del = _addOrGetDelegate<TOwnedDto, TReturnType>(castMethod,ownedClrType);

            var ownedValue = del(dto);
            match.PropertyInfo?.SetValue(entity, ownedValue);
        }

    }
    
}
