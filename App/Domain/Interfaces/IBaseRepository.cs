namespace FairDraw.Listings.App.Domain.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity);
    Task<TEntity?> FindAsync(Guid id);
    Task<IEnumerable<TEntity>> GetPagedFilteredResultAsync(int pageSize, Guid? lastId = null, string? search = null, string? sortBy = null, bool isSortAscending = true);
    void Remove(TEntity entity);
    Task SaveChangesAsync();
    void Update(TEntity entity);
}