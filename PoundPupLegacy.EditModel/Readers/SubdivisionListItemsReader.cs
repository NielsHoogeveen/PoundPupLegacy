namespace PoundPupLegacy.EditModel.Readers;

using Request = SubdivisionListItemsReaderRequest;
using Factory = SubdivisionListItemsReaderFactory;
using Reader = SubdivisionListItemsReader;

public sealed record SubdivisionListItemsReaderRequest: IRequest
{
    public required int CountryId { get; init; }
}

internal sealed class SubdivisionListItemsReaderFactory : EnumerableDatabaseReaderFactory<Request, SubdivisionListItem, Reader>
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
internal sealed class SubdivisionListItemsReader : EnumerableDatabaseReader<Request, SubdivisionListItem>
{
    public SubdivisionListItemsReader(NpgsqlCommand command) : base(command)
    {
    }
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.CountryId, request.CountryId)
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
