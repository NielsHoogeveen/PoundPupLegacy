namespace PoundPupLegacy.CreateModel;

public abstract record TenantNode: IRequest
{
    private TenantNode() { }
    public required int TenantId { get; init; }
    public required string? UrlPath { get; init; }
    public required int? SubgroupId { get; init; }
    public required int PublicationStatusId { get; init; }
    public sealed record ToUpdate : TenantNode, CertainlyIdentifiable
    {
        public required Identification.Certain Identification { get; init; }
    }
    public abstract record ToCreate: TenantNode, PossiblyIdentifiable
    {
        private ToCreate() { }
        public required Identification.Possible Identification { get; init; }
        public sealed record ForNewNode : ToCreate
        {
            public required int? UrlId { get; set; }

            public ForExistingNode ResolveNodeId(int nodeId)
            {
                return new ForExistingNode {
                    TenantId = TenantId,
                    UrlPath = UrlPath,
                    SubgroupId = SubgroupId,
                    PublicationStatusId = PublicationStatusId,
                    UrlId = UrlId ?? nodeId,
                    NodeId = nodeId,
                    Identification = Identification
                };
            }
        }
        public sealed record ForExistingNode : ToCreate
        {
            public required int UrlId { get; set; }
            public required int NodeId { get; set; }
        }
    }
}
