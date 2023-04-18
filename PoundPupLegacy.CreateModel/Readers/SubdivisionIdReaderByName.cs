namespace PoundPupLegacy.CreateModel.Readers;

using Request = SubdivisionIdReaderByNameRequest;

public sealed class SubdivisionIdReaderByNameRequest : IRequest
{
    public required int CountryId { get; init; }
    public required string Name { get; init; }
}
internal sealed class SubdivisionIdReaderByNameFactory : IntDatabaseReaderFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    internal static IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT id
        FROM public.subdivision 
        WHERE country_id = @country_id
        AND name = @name 
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CountryId, request.CountryId),
            ParameterValue.Create(Name, request.Name)
        };
    }

    protected override IntValueReader IntValueReader => IdReader;

    protected override string GetErrorMessage(Request request)
    {
        return $"subdivision with code {request.Name} cannot be found";
    }
}
