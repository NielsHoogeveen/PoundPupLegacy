namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using Request = TopicsDocumentReaderRequest;

public sealed record TopicsDocumentReaderRequest : IRequest
{
    public required int UserId { get; init; }
    public required int TenantId { get; init; }
    public required int Limit { get; init; }
    public required int Offset { get; init; }
    public required string SearchTerm { get; init; }
    public required SearchOption SearchOption { get; init; }
}

internal sealed class TopicsDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, Topics>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NullableIntegerDatabaseParameter LimitParameter = new() { Name = "limit" };
    private static readonly NullableIntegerDatabaseParameter OffsetParameter = new() { Name = "offset" };
    private static readonly SearchOptionDatabaseParameter PatternParameter = new() { Name = "pattern" };

    private static readonly FieldValueReader<Topics> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
        with
        {SharedSql.ACCESSIBLE_PUBLICATIONS_STATUS}
        select
        jsonb_build_object(
            'NumberOfEntries',
            number_of_entries,
            'Entries',
            jsonb_agg(
            	jsonb_build_object(
            		'Title',
            		name,
            		'Path',
            		path,
            		'HasBeenPublished',
            		has_been_published,
                    'PublicationStatusId',
                    publication_status_id
            	)
            ) 
        ) document
        from(
            select
                path, 
                name,
                case 
            	    when publication_status_id = 0 then false
            	    else true
                end has_been_published,	
                publication_status_id,
                count(*) over () number_of_entries
            from(
                select
            	    case 
            		    when tn.url_path is null then '/node/' || tn.url_id
            		    else '/' || url_path
            	    end path,
            	    tm.name,
                    tn.publication_status_id
                from tenant tt
                join system_group sg on sg.id = 0
                join term tm on tm.vocabulary_id = sg.vocabulary_id_tagging
                join node n on n.id = tm.nameable_id
                join tenant_node tn on tn.node_id = n.id and tn.tenant_id = tt.id
                where tt.id = @tenant_id
                and tn.publication_status_id in 
                (
                    select 
                    id 
                    from accessible_publication_status 
                    where tenant_id = tn.tenant_id 
                    and (
                        subgroup_id = tn.subgroup_id 
                        or subgroup_id is null and tn.subgroup_id is null
                    )
                )
                and tm.name ilike @pattern
            ) x 
            limit @limit offset @offset
        ) x
        group by 
        number_of_entries
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
            ParameterValue.Create(LimitParameter, request.Limit),
            ParameterValue.Create(OffsetParameter, request.Offset),
            ParameterValue.Create(PatternParameter, (request.SearchTerm, request.SearchOption)),
        };
    }

    protected override Topics Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
