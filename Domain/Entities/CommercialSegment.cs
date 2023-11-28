namespace Domain.Entities;

public class CommercialSegment : EntityBase<Guid>, IAggregateRoot
{
    public string Name { get; set; }

    public CommercialSegment(string name)
    {
        Name = name;
    }
}