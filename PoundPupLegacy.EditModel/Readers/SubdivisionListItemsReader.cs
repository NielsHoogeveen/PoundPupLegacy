namespace PoundPupLegacy.EditModel.Readers;

using Request = SubdivisionListItemsReaderRequest;

public sealed record SubdivisionListItemsReaderRequest : IRequest
{
    public required int CountryId { get; init; }
}

internal sealed class SubdivisionListItemsReaderFactory : EnumerableDatabaseReaderFactory<Request, SubdivisionListItem>
{
    private static readonly NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };

    private static readonly IntValueReader IdReader = new() { Name = "id" };
    private static readonly StringValueReader NameReader = new() { Name = "name" };

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
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CountryId, request.CountryId)
        };
    }

    protected override SubdivisionListItem Read(NpgsqlDataReader reader)
    {
        return new SubdivisionListItem {
            Id = IdReader.GetValue(reader),
            Name = NameReader.GetValue(reader)
        };
    }
}
