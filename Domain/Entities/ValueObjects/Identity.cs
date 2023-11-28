using Domain.Enums;

namespace Domain.Entities.ValueObjects;

public class Identity : ValueObject
{
    public DocumentType DocumentType { get; init; }
    public string LegalIdentifier { get; init; }

    public Identity(DocumentType documentType, string legalIdentifier)
    {
        DocumentType = documentType;
        LegalIdentifier = legalIdentifier;
    }
}