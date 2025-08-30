using FairDraw.Listings.App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FairDraw.Listings.App.Infrastructure;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<ListingEntity> Listings { get; set; }
}
