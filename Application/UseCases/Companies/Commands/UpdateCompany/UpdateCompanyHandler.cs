using Domain.Services.Companies;
using Domain.Services.Companies.Dto;

namespace Application.UseCases.Companies.Commands.UpdateCompany;

public class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand>
{
    private readonly CompanyUpdateService _companyUpdateService;

    public UpdateCompanyHandler(CompanyUpdateService companyUpdateService)
    {
        _companyUpdateService = companyUpdateService ?? throw new ArgumentNullException(nameof(companyUpdateService));
    }

    public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var dataToUpdate = new CompanyToUpdateDto(request.Id, request.Name, request.Hostname, request.LegalIdentifier,
            request.CommercialSegment);
        await _companyUpdateService.UpdateAsync(dataToUpdate);
        return new Unit();
    }
}