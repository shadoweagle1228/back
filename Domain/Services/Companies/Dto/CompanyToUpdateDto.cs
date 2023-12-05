namespace Domain.Services.Companies.Dto;

public record CompanyToUpdateDto(Guid Id, string Name, string Hostname, string LegalIdentifier, Guid CommercialSegmentId);