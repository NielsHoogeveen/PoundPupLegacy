using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Updaters;

internal sealed class OrganizationUpdaterFactory : DatabaseUpdaterFactory<OrganizationUpdater>
{
    internal static NonNullableIntegerDatabaseParameter NodeId = new() {
        Name = "node_id"
    };
    internal static NonNullableStringDatabaseParameter Title = new() {
        Name = "title"
    };
    internal static NonNullableStringDatabaseParameter Description = new() {
        Name = "description"
    };
    internal static NullableStringDatabaseParameter WebsiteUrl = new() {
        Name = "website_url"
    };
    internal static NullableStringDatabaseParameter EmailAddress = new() {
        Name = "email_address"
    };
    internal static NullableDateTimeRangeDatabaseParameter Established = new() {
        Name = "established"
    };
    internal static NullableDateTimeRangeDatabaseParameter Terminated = new() {
        Name = "terminated"
    };

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

internal sealed class OrganizationUpdater : DatabaseUpdater<OrganizationUpdater.Request>
{

    public OrganizationUpdater(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(OrganizationUpdaterFactory.NodeId, request.NodeId),
            ParameterValue.Create(OrganizationUpdaterFactory.Title, request.Title),
            ParameterValue.Create(OrganizationUpdaterFactory.Description, request.Description),
            ParameterValue.Create(OrganizationUpdaterFactory.WebsiteUrl, request.WebsiteUrl),
            ParameterValue.Create(OrganizationUpdaterFactory.EmailAddress, request.EmailAddress),
            ParameterValue.Create(OrganizationUpdaterFactory.Established, request.EstablishmentDateRange),
            ParameterValue.Create(OrganizationUpdaterFactory.Terminated, request.TerminationDateRange)
        };
    }

    public record Request
    {
        public required int NodeId { get; init; }
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required string? WebsiteUrl { get; init; }
        public required string? EmailAddress { get; init; }
        public required DateTimeRange? EstablishmentDateRange { get; init; }
        public required DateTimeRange? TerminationDateRange { get; init; }

    }
}
