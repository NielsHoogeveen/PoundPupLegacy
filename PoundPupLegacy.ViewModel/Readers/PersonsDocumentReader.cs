namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using Request = PersonsDocumentReaderRequest;
using SearchOption = Common.SearchOption;

public sealed record PersonsDocumentReaderRequest : IRequest
{
    public required int UserId { get; init; }
    public required int TenantId { get; init; }
    public required int Limit { get; init; }
    public required int Offset { get; init; }
    public required string SearchTerm { get; init; }
    public required SearchOption SearchOption { get; init; }

}
internal sealed class PersonsDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, Persons>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter LimitParameter = new() { Name = "limit" };
    private static readonly NonNullableIntegerDatabaseParameter OffsetParameter = new() { Name = "offset" };
    private static readonly SearchOptionDatabaseParameter PatternParameter = new() { Name = "pattern" };

    private static readonly FieldValueReader<Persons> DocumentReader = new() { Name = "document" };

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
            			title,
            			'Path',
            			path,
            			'HasBeenPublished',
            			has_been_published
            		)
            	) 
            ) document
        from(
            select
                path, 
                title,
                case 
            	    when publication_status_id = 0 then false
            	    else true
                end has_been_published,	
                count(*) over () number_of_entries
            from(
                select
            	    case 
            		    when tn.url_path is null then '/node/' || tn.url_id
            		    else '/' || url_path
            	    end path,
            	    n.title,
                    tn.publication_status_id
                from tenant_node tn
                join person p on p.id = tn.node_id
                join node n on n.id = p.id
                where tn.tenant_id = @tenant_id
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
                and n.title ilike @pattern
                order by n.title
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

    protected override Persons Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
