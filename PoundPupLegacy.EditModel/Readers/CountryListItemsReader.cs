namespace PoundPupLegacy.EditModel.Readers;

using Request = CountryListItemsReaderRequest;

public sealed record CountryListItemsReaderRequest : IRequest
{
}

internal sealed class CountryListItemsReaderFactory : EnumerableDatabaseReaderFactory<Request, CountryListItem>
{
    private static readonly IntValueReader IdReader = new() { Name = "id" };
    private static readonly StringValueReader NameReader = new() { Name = "name" };

    public override string Sql => SQL;
    private const string SQL = $"""
        select
            c.id,
            t.name
            from country c
            join term t on t.nameable_id = c.id
            join tenant_node tn on tn.node_id = t.vocabulary_id
            where tn.tenant_id = 1 and tn.url_id = 4126
            
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] { };
    }

    protected override CountryListItem Read(NpgsqlDataReader reader)
    {
        return new CountryListItem {
            Id = IdReader.GetValue(reader),
            Name = NameReader.GetValue(reader)
        };
    }
}
