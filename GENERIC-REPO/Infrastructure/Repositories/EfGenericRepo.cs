using Domain.Common.CustomAttributes;
using Domain.Interfaces;
using Infrastructure.DbConnection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Infrastructure.Repositories;

public class EfGenericRepo<TEntity, TDto>
        where TEntity : class, IEntityModel
        where TDto : IDto, new()

{
    public EfGenericRepo(CookingAppContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<int> SaveChangesInDbAsync() 
        => await _context.SaveChangesAsync();
    
    
    protected static Func<TDto, TEntity> _entityFactory;
    protected static Func<TEntity, TDto> _dtoFactory; 
    protected TDto CastToDto(TEntity e) => _dtoFactory(e);
    protected TEntity CastToEntity(TDto d) => _entityFactory(d);

    protected readonly CookingAppContext _context;
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly IDictionary<Type, Delegate> _delegateCache = new Dictionary<Type, Delegate>();


 
    
    static EfGenericRepo()
    {
        var toEntityMethod =    _getMethod (typeof(TDto), typeof(TEntity));
           var toDtoMethod =    _getMethod (typeof(TEntity), typeof(TDto));

        if (toEntityMethod == null || toDtoMethod == null)
        {
            throw new InvalidOperationException(
                $"Missing explicit conversion operators between {typeof(TEntity)} and {typeof(TDto)}"
            );
        }
        _entityFactory = toEntityMethod
            .CreateDelegate<Func<TDto, TEntity>>();

        _dtoFactory = toDtoMethod
            .CreateDelegate<Func<TEntity, TDto>>();

    }

    protected PropertyInfo _getKeyProperty  (Type targetType)
    {
        var props = targetType.GetProperties();
        var keyProp = props
             .SingleOrDefault(p => p.IsDefined(typeof(KeyIdentifierAttribute), inherit: true))
             ?? throw new InvalidCastException("targetType doesnt have any keyIdentitierAttribute");
        ;

  

        return keyProp;
    }





    protected IList _getCollectionOwnedType<TOwnedDto>(TEntity entity, Type ownedEntityType)
    {

        if (ownedEntityType is null)
        {
            throw new NullReferenceException();
        }


        IEnumerable<INavigation> ownedNavigation =
            _getOwnedNavigation(
            _context.Model.FindEntityType( entity.GetType()),
             (IEntityType n) => n.IsOwned()
                               );

        IEnumerable<INavigation> collectionNavs = ownedNavigation
            .Where(n => _isCollectionNavigation(n));

        INavigation? match = collectionNavs
            .FirstOrDefault(n => n.TargetEntityType.ClrType.Name == ownedEntityType.Name)
            ?? throw new InvalidOperationException  ("No owned collection matches this DTO.");

        var collection = (IList?)match.PropertyInfo?.GetValue(entity)
            ?? throw new NullReferenceException();

        return collection;
    }


    protected  static Type _getOwnedEntityType <TOwnedDto>  (Regex r)
    {
        Assembly a = typeof(TOwnedDto).Assembly;
        string ownedEntityTypeName = r.Replace(typeof(TOwnedDto).Name, "");
        var type = a.GetTypes().SingleOrDefault(t => t.Name == ownedEntityTypeName);
        if (type is null) { throw new NullReferenceException(" no type found"); }
        return type;
    }


    protected static IEnumerable<INavigation> _getOwnedNavigation  (IEntityType? entityType, Predicate<IEntityType> condition)
    {
        return entityType?
             .GetNavigations()
             .Where(n => condition(n.TargetEntityType))
             .ToList() ?? new List<INavigation>();
    }

  
    protected static MethodInfo? _getMethod(Type parameterType, Type methodOwnerType)
    {
        return methodOwnerType.GetMethods(BindingFlags.Public | BindingFlags.Static)
              .SingleOrDefault(m =>
                  m.Name == "op_Explicit" &&
                  m.ReturnType == methodOwnerType &&
                  m.GetParameters().Length == 1 &&
                  m.GetParameters()[0].ParameterType == parameterType );
    }


    protected Func<TEntity, TOwnedDto> _getOrAddDelegateToCache<TOwnedDto>()
    {
        if (_delegateCache.TryGetValue(typeof(TOwnedDto), out var x))
        {
            return (Func<TEntity, TOwnedDto>) x ;
        }

        Type? ownedEntityType = _getOwnedEntityType<TOwnedDto>((new Regex("Dto$")));

        var getMethod = _getMethod(typeof(TEntity), typeof(TOwnedDto)) 
            ?? throw new InvalidCastException("We didnt find any specific explicit operator with this signature");
       
        var delegateToSave =
            getMethod
            .CreateDelegate<Func<TEntity, TOwnedDto>>();
    
        _delegateCache[typeof(TOwnedDto)] = delegateToSave;
        return delegateToSave;
    }
    protected Func<TTargetType, TReturnType> _addOrGetDelegate<TTargetType, TReturnType>(MethodInfo method, Type keyType)
    {
        if (_delegateCache.TryGetValue(keyType, out var value))
        {
            return (Func<TTargetType, TReturnType>)value;
        }
        var del = method.CreateDelegate<Func<TTargetType, TReturnType>>();
        _delegateCache[keyType] = del;
        return del;
    }

    protected bool _isCollectionNavigation(INavigation nav)
    {
        var type = nav.PropertyInfo?.PropertyType;


        if (type == typeof(string) || type is null)
        {
            return false;
        }

        return type.GetInterfaces().Any(i =>
          i.IsGenericType &&
          i.GetGenericTypeDefinition() == typeof(IEnumerable<>)
      );
  
   
    }


}
    //public async Task<TEntity> AddAsync(TDto dto)
    //{
    //    var entity = CastToEntity(dto);
    //    await _dbSet.AddAsync(entity);
    //    await _context.SaveChangesAsync();
    //    return entity;
    //}
    //public async Task UpdateOwnedTypeAsync<TOwnedDto>(int id, TOwnedDto dto)
    //{
    //    var entity = await _dbSet.SingleOrDefaultAsync(x => x.Id == id)
    //        ?? throw new DbNotFoundException("We didnt find any item with this id");

    //    var entityType = _context.Model.FindEntityType(entity.GetType());

    //    var ownedNavigations = entityType?
    //        .GetNavigations()
    //        .Where(n => n.TargetEntityType.IsOwned())
    //        .ToList() ?? new List<INavigation>();

    //    // găsim owned type-ul care corespunde DTO-ului
    //    var match = ownedNavigations
    //        .FirstOrDefault(n => n.TargetEntityType.ClrType.Name + "Dto" == typeof(TOwnedDto).Name);

    //    if (match == null)
    //        throw new InvalidOperationException("DTO does not match any owned type.");

    //    var ownedClrType = match.TargetEntityType.ClrType;


    //    var castMethod = typeof(TOwnedDto)
    //        .GetMethods(BindingFlags.Public | BindingFlags.Static)
    //        .SingleOrDefault(m =>
    //            m.Name == "op_Explicit" &&
    //            m.ReturnType == ownedClrType &&
    //            m.GetParameters().Length == 1 &&
    //            m.GetParameters()[0].ParameterType == typeof(TOwnedDto));

    //    if (castMethod == null)
    //        throw new InvalidOperationException("No explicit cast found for DTO → owned type.");


    //    var ownedValue = castMethod.Invoke(null, new object[] { dto });

    //    // setăm valoarea pe entitate
    //    match.PropertyInfo.SetValue(entity, ownedValue);

    //    await _context.SaveChangesAsync();
    //}


    //public async Task UpdateAsync(TDto dto, int id)
    //{
    //    var toUpdate = await _dbSet
    //        .SingleOrDefaultAsync(e => e.Id == id) ??
    //        throw new DbNotFoundException("We didnt find any item with this id");
    //    var newEntity = CastToEntity(dto);
    //    newEntity.Id = id;
    //    var entries = _context.Entry(toUpdate);


    //    foreach (var prop in entries.Properties)
    //    {
    //        if (
    //             prop.Metadata.IsShadowProperty() ||
    //             prop.Metadata.IsConcurrencyToken ||
    //             prop.Metadata.IsKey() ||
    //             prop.Metadata.IsForeignKey()
    //             )
    //            continue;



    //        var newValue = typeof(TEntity).GetProperty(prop.Metadata.Name)?.GetValue(newEntity);


    //        if (newValue == null)
    //            continue;

    //        prop.CurrentValue = newValue;

    //    }


    //    await _context.SaveChangesAsync();
    //}

    //public async Task DeleteAsync(int id)
    //{
    //    var entity = await _dbSet.SingleOrDefaultAsync(e => e.Id == id) ??
    //                throw new DbNotFoundException("We didnt find any item with this id");
    //    ;

    //    _context.Entry(entity).Property("IsDeleted").CurrentValue = true;
    //    await _context.SaveChangesAsync();
    //}


    //public async Task<IEnumerable<TDto>> GetAllAsync()
    //{
    //    var entities = await _dbSet.ToListAsync();
    //    return entities.Select(CastToDto).ToList();
    //}

    //public async Task<PagedResult<TDto>> GetAllPagedAsync(int page = 1, int pageSize = 10)
    //{
    //    var query = _dbSet.AsQueryable();
    //    var totalItems = await query.CountAsync();

    //    var items = await query
    //    .OrderBy(x => x.Id)
    //    .Skip((page - 1) * pageSize)
    //    .Take(pageSize)
    //    .ToListAsync();

    //    return new PagedResult<TDto>
    //    {
    //        PageNumber = page,
    //        PageSize = pageSize,
    //        TotalItems = totalItems,
    //        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
    //        Items = items.Select(CastToDto).ToList()
    //    };
    //}

    //public async Task<TDto> GeByIdAsync(int id)
    //{
    //    var entity = await _dbSet.SingleOrDefaultAsync(e => e.Id == id)
    //        ?? throw new DbNotFoundException("We didnt find any item with this id");
    //    var dto = CastToDto(entity);
    //    return dto;
    //}


