using FairDraw.Listings.App.Domain.Entities;
using FairDraw.Listings.App.Domain.Interfaces;

namespace FairDraw.Listings.App.Infrastructure.Repositories;

public class ListingRepository(DataContext dataContext) : BaseRepository<ListingEntity>(dataContext), IListingRepository
{

}
