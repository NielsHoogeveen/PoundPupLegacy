namespace PoundPupLegacy.Model;

public interface UserGroup : Identifiable
{
    public string Name { get;  }
    public string Description { get; }
}
