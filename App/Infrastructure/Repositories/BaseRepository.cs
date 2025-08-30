using FairDraw.Listings.App.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FairDraw.Listings.App.Infrastructure.Repositories;

public class BaseRepository<TEntity>(DataContext dataContext) : IBaseRepository<TEntity> where TEntity : class
{
    public async Task AddAsync(TEntity entity)
        => await dataContext.Set<TEntity>().AddAsync(entity);

    public async Task<TEntity?> FindAsync(Guid id)
        => await dataContext.Set<TEntity>().FindAsync(id);

    public async Task<IEnumerable<TEntity>> GetPagedFilteredResultAsync(int pageSize, Guid? lastId = null, string? search = null, string? sortBy = null, bool isSortAscending = true)
    {
        IQueryable<TEntity> query = dataContext.Set<TEntity>();

        if (!string.IsNullOrEmpty(search))
        {
            var stringProperties = typeof(TEntity).GetProperties()
                .Where(p => p.PropertyType == typeof(string));
            foreach (var prop in stringProperties)
            {
                query = query.Where(e =>
                    EF.Functions.Like(EF.Property<string>(e, prop.Name), $"%{search}%"));
            }
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            query = isSortAscending
                ? query.OrderBy(e => EF.Property<object>(e, sortBy))
                : query.OrderByDescending(e => EF.Property<object>(e, sortBy));
        }

        if (lastId.HasValue)
        {
            var idProp = typeof(TEntity).GetProperty("Id");
            if (idProp != null && idProp.PropertyType == typeof(Guid))
            {
                query = query.Where(e => (Guid)EF.Property<object>(e, "Id") != lastId.Value);
            }
        }

        query = query.Take(pageSize);

        return await query.ToListAsync();
    }

    public void Remove(TEntity entity)
        => dataContext.Set<TEntity>().Remove(entity);

    public async Task SaveChangesAsync()
        => await dataContext.SaveChangesAsync();

    public void Update(TEntity entity)
        => dataContext.Set<TEntity>().Update(entity);
}
