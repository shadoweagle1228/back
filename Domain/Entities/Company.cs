using Domain.Entities.ValueObjects;

namespace Domain.Entities;

public class Company : EntityBase<Guid>, IAggregateRoot
{
    public string Name { get; set; }
    public string LegalIdentifier { get; init; }
    public string Hostname { get; set; }
    public AuthorizedAgent AuthorizedAgent { get; set; }
    public Guid CommercialSegmentId { get; set; }
    public CommercialSegment CommercialSegment { get; set; }

    public Company(string name, string legalIdentifier, string hostname, AuthorizedAgent authorizedAgent, Guid commercialSegmentId)
    {
        Name = name;
        LegalIdentifier = legalIdentifier;
        Hostname = hostname;
        AuthorizedAgent = authorizedAgent;
        CommercialSegmentId = commercialSegmentId;
    }
}