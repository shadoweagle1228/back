using Domain.Entities;
using Domain.Entities.Validators;
using Domain.Exceptions.Common;
using Domain.Ports;

namespace Domain.Services.Companies;

[DomainService]
public class CompanyValidationService
{
    private readonly IGenericRepository<Company> _companyRepository;
    private readonly IGenericRepository<CommercialSegment> _commercialSegmentRepository;

    public CompanyValidationService(

        IGenericRepository<Company> companyRepository,
        IGenericRepository<CommercialSegment> commercialSegmentRepository
    )
    {
        _companyRepository = companyRepository;
        _commercialSegmentRepository = commercialSegmentRepository;
    }

    public async Task ValidateExistingHostNameAsync(string host)
    {
        await HostValidator.Validate(host);
        bool alredyExistHost = await _companyRepository.Exist(company => company.Hostname == host);
        if (alredyExistHost)
        {
            string exceptionMessage = string.Format(Messages.AlredyExistException, nameof(host), host);
            throw new ResourceAlreadyExistException(exceptionMessage);
        }
    }

    public async Task ValidateExistingCommercialSegmentAsync(Guid id)
    {
        bool existCommercialSegment = await _commercialSegmentRepository.Exist(commercialSegment => commercialSegment.Id == id);
        if (!existCommercialSegment)
        {
            string exceptionMessage = string.Format(Messages.ResourceNotFoundException, id);
            throw new ResourceNotFoundException(exceptionMessage);
        }
    }

    public async Task ValidateExistingCompanyNameAsync(string name)
    {
        bool alredyExistLegalIdentifier = await _companyRepository.Exist(company => company.Name == name);
        if (alredyExistLegalIdentifier)
        {
            string exceptionMessage = string.Format(Messages.AlredyExistException, nameof(name), name);
            throw new ResourceAlreadyExistException(exceptionMessage);
        }
    }

    public async Task ValidateExistingCompanyLegalIdentifierAsync(string companyLegalIdentifier)
    {
        bool alredyExistLegalIdentifier = await _companyRepository.Exist(company => company.LegalIdentifier == companyLegalIdentifier);
        if (alredyExistLegalIdentifier)
        {
            string exceptionMessage = string.Format(Messages.AlredyExistException, nameof(companyLegalIdentifier), companyLegalIdentifier);
            throw new ResourceAlreadyExistException(exceptionMessage);
        }
    }
}