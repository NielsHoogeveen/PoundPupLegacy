using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Updaters;

using Request = OrganizationUpdaterRequest;
using Factory = OrganizationUpdaterFactory;
using Updater = OrganizationUpdater;

public record OrganizationUpdaterRequest: IRequest
{
    public required int NodeId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string? WebsiteUrl { get; init; }
    public required string? EmailAddress { get; init; }
    public required DateTimeRange? EstablishmentDateRange { get; init; }
    public required DateTimeRange? TerminationDateRange { get; init; }

}
internal sealed class OrganizationUpdaterFactory : DatabaseUpdaterFactory<Request,Updater>
{
    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    internal static NullableStringDatabaseParameter WebsiteUrl = new() { Name = "website_url" };
    internal static NullableStringDatabaseParameter EmailAddress = new() { Name = "email_address" };
    internal static NullableDateRangeDatabaseParameter Established = new() { Name = "established" };
    internal static NullableDateRangeDatabaseParameter Terminated = new() { Name = "terminated" };

    public override string Sql => $"""
        update node 
        set 
            title=@title
        where id = @node_id;
        update nameable 
        set 
            description=@description
        where id = @node_id;
        update organization 
        set 
            website_url=@website_url,
            email_address=@email_address,
            established=@established,
            terminated=@terminated
        where id = @node_id;
        """;

}

internal sealed class OrganizationUpdater : DatabaseUpdater<Request>
{

    public OrganizationUpdater(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(Factory.NodeId, request.NodeId),
            ParameterValue.Create(Factory.Title, request.Title),
            ParameterValue.Create(Factory.Description, request.Description),
            ParameterValue.Create(Factory.WebsiteUrl, request.WebsiteUrl),
            ParameterValue.Create(Factory.EmailAddress, request.EmailAddress),
            ParameterValue.Create(Factory.Established, request.EstablishmentDateRange),
            ParameterValue.Create(Factory.Terminated, request.TerminationDateRange)
        };
    }
}
