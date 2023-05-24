namespace PoundPupLegacy.CreateModel;

public sealed record ExistingTenantNode : ImmediatelyIdentifiableTenantNode
{
    public required int Id { get; init; }

    public required string? UrlPath { get; init; }

    public required int? SubgroupId { get; init; }

    public required int PublicationStatusId { get; init; }

}

public sealed record NewTenantNodeForNewNode : EventuallyIdentifiableTenantNodeForNewNode
{
    public required int? Id { get; set; }
    public required int TenantId { get; init; }

    public required int? UrlId { get; set; }

    public required string? UrlPath { get; init; }

    public required int? NodeId { get; set; }

    public required int? SubgroupId { get; init; }

    public required int PublicationStatusId { get; init; }

}

public sealed record NewTenantNodeForExistingNode : EventuallyIdentifiableTenantNodeForExistingNode
{
    public required int? Id { get; set; }
    public required int TenantId { get; init; }

    public required int UrlId { get; set; }

    public required string? UrlPath { get; init; }

    public required int NodeId { get; set; }

    public required int? SubgroupId { get; init; }

    public required int PublicationStatusId { get; init; }

}
public interface ImmediatelyIdentifiableTenantNode : TenantNode, ImmediatelyIdentifiable
{
}
public interface EventuallyIdentifiableTenantNodeForNewNode : TenantNode, EventuallyIdentifiable
{
    int TenantId { get; }

    int? UrlId { get; }

    int? NodeId { get; }
}
public interface EventuallyIdentifiableTenantNodeForExistingNode : TenantNode, EventuallyIdentifiable
{
    int TenantId { get; }
    int UrlId { get; }

    int NodeId { get; }
}
public interface TenantNode
{
    string? UrlPath { get; }

    int? SubgroupId { get; }

    int PublicationStatusId { get; }

}