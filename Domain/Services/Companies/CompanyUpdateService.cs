using Domain.Entities;
using Domain.Exceptions.Common;
using Domain.Ports;
using Domain.Services.Companies.Dto;

namespace Domain.Services.Companies;

[DomainService]
public class CompanyUpdateService
{
    private readonly IGenericRepository<Company> _companyRepository;
    private readonly CompanyValidationService _companyValidationService;

    public CompanyUpdateService(IGenericRepository<Company> companyRepository, CompanyValidationService companyValidationService)
    {
        _companyRepository = companyRepository;
        _companyValidationService = companyValidationService;
    }

    public async Task UpdateAsync(CompanyToUpdateDto dataToUpdate)
    {
        var company = await FindCompanyById(dataToUpdate.Id);
        await ValidateDataToUpdate(dataToUpdate, company);
        company.Update(
            dataToUpdate.LegalIdentifier,
            dataToUpdate.Name,
            dataToUpdate.Hostname,
            dataToUpdate.CommercialSegmentId
        );
        await _companyRepository.UpdateAsync(company);
    }

    private async Task ValidateDataToUpdate(CompanyToUpdateDto dataToUpdate, Company company)
    {
        if (company.CommercialSegmentId != dataToUpdate.CommercialSegmentId)
        {
            await _companyValidationService.ValidateExistingCommercialSegmentAsync(dataToUpdate.CommercialSegmentId);
        }

        if (company.Name != dataToUpdate.Name)
        {
            await _companyValidationService.ValidateExistingCompanyNameAsync(dataToUpdate.Name);
        }

        if (company.LegalIdentifier != dataToUpdate.LegalIdentifier)
        {
            await _companyValidationService.ValidateExistingCompanyLegalIdentifierAsync(dataToUpdate.LegalIdentifier);
        }

        if (company.Hostname != dataToUpdate.Hostname)
        {
            await _companyValidationService.ValidateExistingHostNameAsync(dataToUpdate.Hostname);
        }
    }

    private async Task<Company> FindCompanyById(Guid id)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        return company ?? throw new ResourceNotFoundException(string.Format(Messages.ResourceNotFoundException, id));
    }
}