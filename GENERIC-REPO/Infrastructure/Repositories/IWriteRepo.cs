namespace Infrastructure.Repositories
{
    public interface IWriteRepo<TDto> where TDto : IDto, new()
    {
        Task DeleteAsync(int id);
        Task UpdateEntity(TDto dto, int id);
        Task UpdateOwnedTypeAsync<TOwnedDto, TParameterType>(int id, TOwnedDto dto);
        Task UpdateComplexOwnedTypeAsync<TOwnedDto, TUpdateTargetType>(int id, TOwnedDto dto);
    }
}