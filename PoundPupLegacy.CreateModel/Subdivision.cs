namespace PoundPupLegacy.CreateModel;

public interface ImmediatelyIdentifiableSubdivision : Subdivision, ImmediatelyIdentifiableGeographicalEntity
{
}

public interface EventuallyIdentifiableSubdivision: Subdivision, EventuallyIdentifiableGeographicalEntity
{
}
public interface Subdivision : GeographicalEntity
{
    string Name { get; }

    int CountryId { get; }

    int SubdivisionTypeId { get; }
}

public abstract record NewSubdivisionBase: NewNameableBase, EventuallyIdentifiableSubdivision
{
    public required string Name { get; init; }

    public required int CountryId { get; init; }

    public required int SubdivisionTypeId { get; init; }

}

public abstract record ExistingSubdivisionBase : ExistingNameableBase, ImmediatelyIdentifiableSubdivision
{
    public required string Name { get; init; }

    public required int CountryId { get; init; }

    public required int SubdivisionTypeId { get; init; }

}