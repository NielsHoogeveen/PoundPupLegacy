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

public record EditListItemBase<T> : EditListItem, IComparable<T>
    where T: EditListItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }

    public int CompareTo(T? other)
    {
        if (other is null)
            return -1;
        return Name.CompareTo(other.Name);
    }
}