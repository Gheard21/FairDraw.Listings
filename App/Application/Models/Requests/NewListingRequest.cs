namespace FairDraw.Listings.App.Application.Models.Requests;

public record NewListingRequest
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public double Price { get; set; }
}
