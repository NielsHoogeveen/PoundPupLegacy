namespace PoundPupLegacy.CreateModel;

public abstract record InterCountryRelation : Node
{
    private InterCountryRelation() { }
    public required InterCountryRelationDetails InterCountryRelationDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToCreate : InterCountryRelation, NodeToCreate
    {
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override Identification Identification => IdentificationForCreate;

        public override NodeDetails NodeDetails => NodeDetailsForCreate;
    }
    public sealed record ToUpdate : InterCountryRelation, NodeToUpdate
    {
        public override Identification Identification => IdentificationCertain;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
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