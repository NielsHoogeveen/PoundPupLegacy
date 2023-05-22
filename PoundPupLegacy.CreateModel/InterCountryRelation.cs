namespace PoundPupLegacy.CreateModel;

public sealed record NewInterCountryRelation : NewNodeBase, EventuallyIdentifiableInterCountryRelation
{
    public required int InterCountryRelationTypeId { get; init; }
    public required int CountryIdFrom { get; init; }
    public required int CountryIdTo { get; init; }
    public required DateTimeRange? DateTimeRange { get; init; }
    public required int? NumberOfChildrenInvolved { get; init; }
    public required decimal? MoneyInvolved { get; init; }
    public required int? DocumentIdProof { get; init; }

}
public sealed record ExistingInterCountryRelation : ExistingNodeBase, ImmediatelyIdentifiableInterCountryRelation
{
    public required int InterCountryRelationTypeId { get; init; }
    public required int CountryIdFrom { get; init; }
    public required int CountryIdTo { get; init; }
    public required DateTimeRange? DateTimeRange { get; init; }
    public required int? NumberOfChildrenInvolved { get; init; }
    public required decimal? MoneyInvolved { get; init; }
    public required int? DocumentIdProof { get; init; }

}
public interface ImmediatelyIdentifiableInterCountryRelation : InterCountryRelation, ImmediatelyIdentifiableNode
{
}
public interface EventuallyIdentifiableInterCountryRelation : InterCountryRelation, EventuallyIdentifiableNode
{
}
public interface InterCountryRelation : Node
{
    int InterCountryRelationTypeId { get; }
    int CountryIdFrom { get; }
    int CountryIdTo { get; }
    DateTimeRange? DateTimeRange { get; }
    int? NumberOfChildrenInvolved { get; }
    decimal? MoneyInvolved { get; }
    int? DocumentIdProof { get; }

}
