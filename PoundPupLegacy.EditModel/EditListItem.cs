namespace PoundPupLegacy.EditModel;

public interface Named {
    public string Name { get; }

}
public interface EditListItem: Named
{
    public int? Id { get; }

}
