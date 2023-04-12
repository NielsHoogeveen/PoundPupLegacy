namespace PoundPupLegacy.CreateModel.Readers;

using Factory = SubdivisionIdReaderByNameFactory;
using Reader = SubdivisionIdReaderByName;
public sealed class SubdivisionIdReaderByNameFactory : DatabaseReaderFactory<Reader>
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

}
public sealed class SubdivisionIdReaderByName : IntDatabaseReader<Reader.Request>
{
    public record Request
    {
        public required int CountryId { get; init; }
        public required string Name { get; init; }

    }
    internal SubdivisionIdReaderByName(NpgsqlCommand command) : base(command) { }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.CountryId, request.CountryId),
            ParameterValue.Create(Factory.Name, request.Name)
        };
    }

    protected override IntValueReader IntValueReader => Factory.IdReader;

    protected override string GetErrorMessage(Request request)
    {
        return $"subdivision with code {request.Name} cannot be found";
    }
}
