namespace PoundPupLegacy.CreateModel;
public interface ImmediatelyIdentifiableISOCodedSubdivision : ISOCodedSubdivision, ImmediatelyIdentifiableSubdivision, ImmediatelyIdentifiablePoliticalEntity
{
}
public interface EventuallyIdentifiableISOCodedSubdivision: ISOCodedSubdivision, EventuallyIdentifiableSubdivision, EventuallyIdentifiablePoliticalEntity
{
}
public interface ISOCodedSubdivision : Subdivision, PoliticalEntity
{
    string ISO3166_2_Code { get; }
}
public abstract record NewISOCodedSubdivisionBase: NewSubdivisionBase, EventuallyIdentifiableISOCodedSubdivision 
{
    public required string ISO3166_2_Code { get; init; }

    public required int? FileIdFlag { get; init; }
}
public abstract record ExistingISOCodedSubdivisionBase : ExistingSubdivisionBase, ImmediatelyIdentifiableISOCodedSubdivision
{
    public required string ISO3166_2_Code { get; init; }

    public required int? FileIdFlag { get; init; }
}
