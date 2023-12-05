using System.Text.Json.Serialization;

namespace Application.UseCases.Companies.Commands.UpdateCompany;

public record UpdateCompanyCommand: IRequest<Unit>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string LegalIdentifier { get; set; }
    public string Name { get; set; }
    public string Hostname { get; set; }
    public Guid CommercialSegment { get; set; }
}