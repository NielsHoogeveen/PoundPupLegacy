namespace PoundPupLegacy.CreateModel;
public interface ImmediatelyIdentifiablePoliticalEntity : ImmediatelyIdentifiableGeographicalEntity, PoliticalEntity
{
}

public interface EventuallyIdentifiablePoliticalEntity: PoliticalEntity, EventuallyIdentifiableGeographicalEntity
{
}

public interface PoliticalEntity : GeographicalEntity
{
    int? FileIdFlag { get; }
}

public record NewPoliticalEntityBase: NewNameableBase, EventuallyIdentifiablePoliticalEntity
{
    public required int? FileIdFlag { get; init; }
}
public record ExistingPoliticalEntityBase : ExistingNameableBase, ImmediatelyIdentifiablePoliticalEntity
{
    public required int? FileIdFlag { get; init; }
}
