namespace PoundPupLegacy.ViewModel.Readers;

using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using Request = SubgroupsReaderRequest;

public sealed record SubgroupsReaderRequest : IRequest
{
    public required int TenantId { get; init; }
}
internal sealed class SubgroupsReaderFactory : SingleItemDatabaseReaderFactory<Request, List<Subgroup>>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };

    private static readonly FieldValueReader<List<Subgroup>> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
        select
        jsonb_agg(
        	jsonb_build_object(
        		'Name',
        		name,
        		'Path',
        		path,
        		'Description',
        		description
        	)	
        	order by name
        ) document
        from(
        	select
        	ug.name,
        	ug.description,
        	'/group/' || sg.id path
        	from subgroup sg
        	join user_group ug on ug.id = sg.id
        	where tenant_id = @tenant_id
        ) x
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
        };
    }

    protected override List<Subgroup> Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
