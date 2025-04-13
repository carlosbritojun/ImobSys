using ImobSys.Api.Common.Domain;

namespace ImobSys.Api.Modules.Admin.Domain;

public sealed class PropertyType : Entity<Guid>
{
    public PropertyType(Guid id, string name, string description)
        : base(id)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}