namespace PoundPupLegacy.CreateModel;

public abstract record TenantNode: IRequest
{
    public required string? UrlPath { get; init; }

    public required int? SubgroupId { get; init; }

    public required int PublicationStatusId { get; init; }

    public sealed record TenantNodeToUpdate : TenantNode, ImmediatelyIdentifiable
    {

        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }

        public Identification Identification => IdentificationForUpdate;
    }

    public sealed record TenantNodeToCreateForNewNode : TenantNode, EventuallyIdentifiable
    {
        public required int TenantId { get; init; }

        public required int? UrlId { get; set; }

        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }

        public Identification Identification => IdentificationForCreate;

        public TenantNodeToCreateForExistingNode ResolveNodeId(int nodeId)
        {
            return new TenantNodeToCreateForExistingNode {
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

    public sealed record TenantNodeToCreateForExistingNode : TenantNode, EventuallyIdentifiable
    {
        public required int TenantId { get; init; }

        public required int UrlId { get; set; }

        public required int NodeId { get; set; }

        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }

        public Identification Identification => IdentificationForCreate;
    }
}
