using ImobSys.Api.Common.Domain;

namespace ImobSys.Api.Modules.Admin.Domain;

public sealed class Property : Entity<Guid>
{
    public Property(Guid id, string name, string description, Guid typeId)
        : base(id)
    {
        Name = name;
        Description = description;
        TypeId = typeId;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public Guid TypeId { get; private set; }
    public PropertyType Type { get; private set; }

    public void Update(string name, string description, Guid typeId)
    {
        Name = name;
        Description = description;
        TypeId = typeId;
    }
}
