namespace PoundPupLegacy.EditModel;

public interface Named
{
    public string Name { get; }

}
public interface NamedOnly : Named
{
}
public interface EditListItem : Named
{
    public int Id { get; }

}
