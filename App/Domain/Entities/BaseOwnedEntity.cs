namespace FairDraw.Listings.App.Domain.Entities;

public class BaseOwnedEntity(string tenantId)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string TenantId { get; private set; } = tenantId;

    protected BaseOwnedEntity() : this(string.Empty) { }
}
