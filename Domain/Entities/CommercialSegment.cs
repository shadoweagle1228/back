namespace Domain.Entities;

public class CommercialSegment : EntityBase<Guid>, IAggregateRoot
{
    public string Name { get; set; }
    public string Description { get; set; }

    public CommercialSegment(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public void Update(string name, string description)
    {
        if (Name.Equals(name) is not true) Name = name;
        if (Description.Equals(description) is not true) Description = description;
    }
}