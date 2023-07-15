namespace PoundPupLegacy.DomainModel;

public abstract record InterCountryRelation : Node
{
    private InterCountryRelation() { }
    public required InterCountryRelationDetails InterCountryRelationDetails { get; init; }
    public sealed record ToCreate : InterCountryRelation, NodeToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
    }
    public sealed record ToUpdate : InterCountryRelation, NodeToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
    }
}

public sealed record InterCountryRelationDetails
{
    public required int InterCountryRelationTypeId { get; init; }
    public required int CountryIdFrom { get; init; }
    public required int CountryIdTo { get; init; }
    public required DateTimeRange? DateTimeRange { get; init; }
    public required int? NumberOfChildrenInvolved { get; init; }
    public required decimal? MoneyInvolved { get; init; }
    public required int? DocumentIdProof { get; init; }

}