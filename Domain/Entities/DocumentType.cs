namespace Domain.Entities;

public class DocumentType : EntityBase<Guid>, IAggregateRoot
{
    public string Code { get; set; }
    public string Name { get; set; }

    public DocumentType(string code, string name)
    {
        Code = code;
        Name = name;
    }
}