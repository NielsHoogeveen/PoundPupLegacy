namespace PoundPupLegacy.CreateModel.Readers;

using Request = SubdivisionIdReaderByNameRequest;
using Factory = SubdivisionIdReaderByNameFactory;
using Reader = SubdivisionIdReaderByName;

public sealed class SubdivisionIdReaderByNameRequest : IRequest
{
    public required int CountryId { get; init; }
    public required string Name { get; init; }
}
internal sealed class SubdivisionIdReaderByNameFactory : MandatorySingleItemDatabaseReaderFactory<Request, int, Reader>
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
internal sealed class SubdivisionIdReaderByName : IntDatabaseReader<Request>
{
    public SubdivisionIdReaderByName(NpgsqlCommand command) : base(command) { }

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
