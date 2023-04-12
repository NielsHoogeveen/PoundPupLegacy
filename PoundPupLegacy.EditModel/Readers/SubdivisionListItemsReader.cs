namespace PoundPupLegacy.EditModel.Readers;

using Factory = SubdivisionListItemsReaderFactory;
using Reader = SubdivisionListItemsReader;
public class SubdivisionListItemsReaderFactory : DatabaseReaderFactory<Reader>
{
    internal static NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };

    internal static readonly IntValueReader IdReader = new() { Name = "id" };
    internal static readonly StringValueReader NameReader = new() { Name = "name" };

    public override string Sql => SQL;
    private const string SQL = $"""
        select
            s.id,
            s.name
            from subdivision s
            join bottom_level_subdivision bls on bls.id = s.id
            where s.country_id = @country_id
            order by s.name
        """;
}
public class SubdivisionListItemsReader : EnumerableDatabaseReader<int, SubdivisionListItem>
{
    internal SubdivisionListItemsReader(NpgsqlCommand command) : base(command)
    {
    }
    protected override IEnumerable<ParameterValue> GetParameterValues(int request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.CountryId, request)
        };
    }

    protected override SubdivisionListItem Read(NpgsqlDataReader reader)
    {
        return new SubdivisionListItem {
            Id = Factory.IdReader.GetValue(reader),
            Name = Factory.NameReader.GetValue(reader)
        };
    }
}
