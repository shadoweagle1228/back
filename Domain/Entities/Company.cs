using Domain.Entities.ValueObjects;
using Domain.Enums;

namespace Domain.Entities;

public class Company : EntityBase<Guid>, IAggregateRoot
{
    public string Name { get; private set; }
    public string LegalIdentifier { get; private set; }
    public string Hostname { get; private set; }
    public AuthorizedAgent AuthorizedAgent { get; set; }
    public Guid CommercialSegmentId { get; private set; }
    public CommercialSegment CommercialSegment { get; set; }
    public CompanyState State { get; private set; }
    public Company() {}

    public Company
    (
        Guid id,
        string name,
        string legalIdentifier,
        string hostname,
        AuthorizedAgent authorizedAgent,
        Guid commercialSegmentId
    )
    {
        Id = id;
        Name = name;
        LegalIdentifier = legalIdentifier;
        Hostname = hostname;
        AuthorizedAgent = authorizedAgent;
        CommercialSegmentId = commercialSegmentId;
        State = CompanyState.Active;
    }

    public void Update(string legalIdentifier, string name, string hostname, Guid commercialSegmentId)
    {
        LegalIdentifier = legalIdentifier;
        Name = name;
        Hostname = hostname;
        CommercialSegmentId = commercialSegmentId;
    }

}