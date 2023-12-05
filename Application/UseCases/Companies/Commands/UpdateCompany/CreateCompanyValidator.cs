namespace Application.UseCases.Companies.Commands.UpdateCompany;

public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyValidator()
    {
        RuleFor(_ => _.Name).NotNull().NotEmpty().MinimumLength(3).MaximumLength(30);
        RuleFor(_ => _.Hostname).NotNull().NotEmpty().MinimumLength(5).MaximumLength(255);
        RuleFor(_ => _.LegalIdentifier).NotNull().NotEmpty().MinimumLength(5).MaximumLength(30);
        RuleFor(_ => _.CommercialSegment).NotNull();
    }
}