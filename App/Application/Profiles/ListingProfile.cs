using AutoMapper;
using FairDraw.Listings.App.Application.Models.Requests;
using FairDraw.Listings.App.Application.Models.Responses;
using FairDraw.Listings.App.Domain.Entities;

namespace FairDraw.Listings.App.Application.Profiles;

public class ListingProfile : Profile
{
    public ListingProfile()
    {
        CreateMap<NewListingRequest, ListingEntity>();
        CreateMap<ListingEntity, ListingResponse>();
    }
}
