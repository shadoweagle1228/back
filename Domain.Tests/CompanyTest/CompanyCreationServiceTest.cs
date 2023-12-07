using System.Linq.Expressions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Ports;
using Domain.Services.Companies;
using Domain.Tests.BuilderEntities;

namespace Domain.Tests.CompanyTest;

public class CompanyCreationServiceTest
{
    private readonly IGenericRepository<Company> _companyRepository;
    private readonly IGenericRepository<DocumentType> _documentTypeRespository;
    private readonly IGenericRepository<CommercialSegment> _commercialSegmentRespository;
    private readonly CompanyCreationService _companyCreationService;

    public CompanyCreationServiceTest()
    {
        _companyRepository = Substitute.For<IGenericRepository<Company>>();
        _documentTypeRespository = Substitute.For<IGenericRepository<DocumentType>>();
        _commercialSegmentRespository = Substitute.For<IGenericRepository<CommercialSegment>>();
        var companyValidationService = new CompanyValidationService(_companyRepository, _commercialSegmentRespository, _documentTypeRespository);
        _companyCreationService = new CompanyCreationService(_companyRepository, companyValidationService);
    }
    
    [Fact]
    public async Task CreateNewCompanyWithValidData_ShouldAddToRepository()
    {
        // Arrange
        var company = new CompanyBuilder().Build();
        _companyRepository.AddAsync(Arg.Any<Company>()).Returns(company);
        _companyRepository.Exist(Arg.Any<Expression<Func<Company, bool>>>()).Returns(Task.FromResult(false));

        // Act
        await _companyCreationService.CreateAsync(company);

        // Assert
        await _companyRepository.Received(4).Exist(Arg.Any<Expression<Func<Company, bool>>>());
        await _companyRepository.Received().AddAsync(Arg.Is<Company>(c =>
            c.AuthorizedAgent.Email == company.AuthorizedAgent.Email &&
            c.AuthorizedAgent.Identity.LegalIdentifier == company.AuthorizedAgent.Identity.LegalIdentifier &&
            c.Hostname == company.Hostname &&
            c.Name == company.Name &&
            c.LegalIdentifier == company.LegalIdentifier));
    }
    
    [Fact]
    public async Task CreateNewCompanyWithAuthorizedAgentHaveInvalidEmail_ShouldThrowInvalidEmailException()
    {
        // Arrange
        const string invalidEmail = "email@test";
        var authorizedAgent = new AuthorizedAgentBuilder().WithEmail(invalidEmail).Build();
        var company = new CompanyBuilder().WithAuthorizedAgent(authorizedAgent).Build();

        // Act - Assert
        await Assert.ThrowsAsync<InvalidEmailException>(async () => await _companyCreationService.CreateAsync(company));
    }
    
    [Fact]
    public async Task CreateNewCompanyWithHostNameIsInvalid_ShouldThrowResourceAlreadyExistException()
    {
        // Arrange
        const string invalidHostName = "test";
        var company = new CompanyBuilder().WithHostname(invalidHostName).Build();

        // Act
        var exception = await Record.ExceptionAsync(() => _companyCreationService.CreateAsync(company));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<InvalidHostNameException>(exception);
        Assert.Equal(Messages.InvalidHostException, exception.Message);
    }
}