namespace FairDraw.Listings.App.Domain.Entities;

public class ListingEntity(string tenantId, string name, double price, string? description) : BaseOwnedEntity(tenantId)
{
    public string Name { get; set; } = name;
    public string? Description { get; set; } = description;
    public double Price { get; set; } = price;
}
