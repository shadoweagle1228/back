using Domain.Entities.ValueObjects;
using Domain.Enums;
using Domain.Services.Companies.Dto;

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
        State = CompanyState.ENABLE;
    }

    public void Update(
        string hostname,
        CompanyState state,
        Guid commercialSegmentId,
        AuthorizeAgentToUpdateDto authorizeAgentToUpdate
    )
    {
        Hostname = hostname;
        State = state;
        CommercialSegmentId = commercialSegmentId;
        AuthorizedAgent.Update(authorizeAgentToUpdate);
    }

}