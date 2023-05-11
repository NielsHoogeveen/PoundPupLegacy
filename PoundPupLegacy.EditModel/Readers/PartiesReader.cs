namespace PoundPupLegacy.EditModel.Readers;

using Request = PartiesReaderRequest;
using SearchOption = Common.SearchOption;

public sealed record PartiesReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required string SearchString { get; init; }
}

internal sealed class PartiesReaderFactory : EnumerableDatabaseReaderFactory<Request, PartyListItem>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly SearchOptionDatabaseParameter SearchOptionParameter = new() { Name = "search_option" };
    private static readonly NonNullableStringDatabaseParameter SearchStringParameter = new() { Name = "search_string" };

    private static readonly IntValueReader IdReader = new() { Name = "id" };
    private static readonly StringValueReader NameReader = new() { Name = "name" };

    public override string Sql => SQL;

    const string SQL = """
        select
        distinct
        id,
        name
        from(
            select
            n.id,
            n.title name
            from node n 
            left join organization o on o.id = n.id
            left join person p on p.id = n.id
            where n.title = @search_string
            and not(o.id is null and p.id is null)
            union
            select
            n.id id,
            n.title name
            from node n 
            left join organization o on o.id = n.id
            left join person p on p.id = n.id
            where n.title ilike @search_option
            and not(o.id is null and p.id is null)
            LIMIT 50
        ) x
        """;


    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(SearchStringParameter, request.SearchString),
            ParameterValue.Create(SearchOptionParameter, (request.SearchString, SearchOption.StartsWith)),
        };
    }

    protected override PartyListItem Read(NpgsqlDataReader reader)
    {
        return new PartyListItem {
            Id = IdReader.GetValue(reader),
            Name = NameReader.GetValue(reader),
        };
    }

}
