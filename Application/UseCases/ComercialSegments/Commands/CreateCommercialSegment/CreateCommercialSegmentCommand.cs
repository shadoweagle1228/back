using System.Text.Json.Serialization;

namespace Application.UseCases.ComercialSegments.Commands.CreateCommercialSegment;

public record CreateCommercialSegmentCommand : IRequest<Unit>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
