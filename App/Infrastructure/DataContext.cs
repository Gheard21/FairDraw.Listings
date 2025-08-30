using Microsoft.EntityFrameworkCore;

namespace FairDraw.Listings.App.Infrastructure;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{

}
