using System.Text.Json.Serialization;

namespace Application.UseCases.Companies.Commands.CreateCompany;

public record CreateCompanyCommand : IRequest<Unit>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string LegalIdentifier { get; set; }
    public string Hostname { get; set; }
    public CreateAuthorizeAgentCommand AuthorizedAgent { get; set; }
    public Guid CommercialSegment { get; set; }
}


public record CreateAuthorizeAgentCommand(
    string Name,
    string Surname,
    string Email,
    CreateIdentityCommand Identity
);

public record CreateIdentityCommand(
    string DocumentType,
    string LegalIdentifier
);
