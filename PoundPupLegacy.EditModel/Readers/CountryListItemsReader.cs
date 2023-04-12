namespace PoundPupLegacy.EditModel.Readers;

using Factory = CountryListItemsReaderFactory;
using Reader = CountryListItemsReader;

public sealed class CountryListItemsReaderFactory : DatabaseReaderFactory<Reader>
{
    internal static readonly IntValueReader IdReader = new() { Name = "id" };
    internal static readonly StringValueReader NameReader = new() { Name = "name" };

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
}
public sealed class CountryListItemsReader : EnumerableDatabaseReader<Reader.Request, CountryListItem>
{
    public record Request { }
    public CountryListItemsReader(NpgsqlCommand command) : base(command)
    {
    }


    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] { };
    }

    protected override CountryListItem Read(NpgsqlDataReader reader)
    {
        return new CountryListItem {
            Id = Factory.IdReader.GetValue(reader),
            Name = Factory.NameReader.GetValue(reader)
        };
    }
}
