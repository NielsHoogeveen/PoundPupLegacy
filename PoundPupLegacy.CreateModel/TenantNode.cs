namespace PoundPupLegacy.CreateModel;

public abstract record TenantNode: IRequest
{
    public required string? UrlPath { get; init; }
    public required int? SubgroupId { get; init; }
    public required int PublicationStatusId { get; init; }
    public sealed record ToUpdate : TenantNode, CertainlyIdentifiable
    {
        public required Identification.Certain IdentificationCertain { get; init; }
        public Identification Identification => IdentificationCertain;
    }
    public sealed record ToCreateForNewNode : TenantNode, PossiblyIdentifiable
    {
        public required int TenantId { get; init; }
        public required int? UrlId { get; set; }
        public required Identification.Possible IdentificationForCreate { get; init; }
        public Identification Identification => IdentificationForCreate;
        public ToCreateForExistingNode ResolveNodeId(int nodeId)
        {
            return new ToCreateForExistingNode {
                TenantId = TenantId,
                UrlPath = UrlPath,
                SubgroupId = SubgroupId,
                PublicationStatusId = PublicationStatusId,
                UrlId = UrlId ?? nodeId,
                NodeId = nodeId,
                IdentificationForCreate = IdentificationForCreate
            };
        }
    }
    public sealed record ToCreateForExistingNode : TenantNode, PossiblyIdentifiable
    {
        public required int TenantId { get; init; }
        public required int UrlId { get; set; }
        public required int NodeId { get; set; }
        public required Identification.Possible IdentificationForCreate { get; init; }
        public Identification Identification => IdentificationForCreate;
    }
}
