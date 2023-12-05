using Domain.Entities;
using Domain.Entities.Validators;
using Domain.Exceptions.Common;
using Domain.Ports;

namespace Domain.Services.Companies;

[DomainService]
public class CompanyCreationService
{
    private readonly IGenericRepository<Company> _companyRepository;
    private readonly IGenericRepository<DocumentType> _documentTypeRepository;
    private readonly CompanyValidationService _companyValidationService;

    public CompanyCreationService
    (
        IGenericRepository<Company> companyRepository,
        IGenericRepository<DocumentType> documentTypeRepository,
        CompanyValidationService companyValidationService
    )
    {
        _companyRepository = companyRepository;
        _documentTypeRepository = documentTypeRepository;
        _companyValidationService = companyValidationService;
    }

    public async Task CreateAsync(Company company)
    {
        await ValidateExistingAuthorizedAgentEmailAsync(company.AuthorizedAgent.Email);
        await ValidateExistDocumentTypeAsync(company.AuthorizedAgent.Identity.DocumentType);
        await ValidateExistingAuthorizedAgentLegalIdentifierAsync(company.AuthorizedAgent.Identity.LegalIdentifier);
        await _companyValidationService.ValidateExistingHostNameAsync(company.Hostname);
        await _companyValidationService.ValidateExistingCompanyNameAsync(company.Name);
        await _companyValidationService.ValidateExistingCompanyLegalIdentifierAsync(company.LegalIdentifier);
        await _companyValidationService.ValidateExistingCommercialSegmentAsync(company.CommercialSegmentId);
        await _companyRepository.AddAsync(company);
    }

    private async Task ValidateExistingAuthorizedAgentEmailAsync(string email)
    {
        await EmailValidator.Validate(email);
        bool alredyExistEmail = await _companyRepository.Exist(company => company.AuthorizedAgent.Email == email);
        if (alredyExistEmail)
        {
            string exceptionMessage = string.Format(Messages.AlredyExistException, nameof(email), email);
            throw new ResourceAlreadyExistException(exceptionMessage);
        }
    }

    private async Task ValidateExistDocumentTypeAsync(string codeDocumentType)
    {
        bool existDocumentType = await _documentTypeRepository.Exist(documentType => documentType.Code == codeDocumentType);
        if (!existDocumentType)
        {
            string exceptionMessage = string.Format(Messages.AlredyExistException, codeDocumentType);
            throw new ResourceNotFoundException(exceptionMessage);
        }
    }

    private async Task ValidateExistingAuthorizedAgentLegalIdentifierAsync(string authorizedAgentLegalIdentifier)
    {
        bool alredyExistLegalIdentifier = await _companyRepository.Exist(company => company.AuthorizedAgent.Identity.LegalIdentifier == authorizedAgentLegalIdentifier);
        if (alredyExistLegalIdentifier)
        {
            string exceptionMessage = string.Format(Messages.AlredyExistException, nameof(authorizedAgentLegalIdentifier), authorizedAgentLegalIdentifier);
            throw new ResourceAlreadyExistException(exceptionMessage);
        }
    }
}