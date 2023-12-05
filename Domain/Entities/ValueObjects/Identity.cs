namespace Domain.Entities.ValueObjects;

public class Identity : ValueObject
{
    public string DocumentType { get; init; }
    public string LegalIdentifier { get; init; }

    public Identity()
    {
        
    }

    public Identity(string documentType, string legalIdentifier)
    {
        DocumentType = documentType;
        LegalIdentifier = legalIdentifier;
    }
}