namespace PoundPupLegacy.CreateModel.Readers;

using Factory = SubdivisionIdReaderByIso3166CodeFactory;
using Reader = SubdivisionIdReaderByIso3166Code;

public sealed class SubdivisionIdReaderByIso3166CodeFactory : DatabaseReaderFactory<Reader>
{
    internal static NonNullableStringDatabaseParameter Iso3166Code = new() { Name = "iso_3166_2_code" };

    internal static IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT id
        FROM public.iso_coded_subdivision 
        WHERE iso_3166_2_code = @iso_3166_2_code 
        """;

}
public sealed class SubdivisionIdReaderByIso3166Code : IntDatabaseReader<string>
{
    internal SubdivisionIdReaderByIso3166Code(NpgsqlCommand command) : base(command) { }

    protected override IEnumerable<ParameterValue> GetParameterValues(string request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Iso3166Code, request)
        };
    }

    protected override IntValueReader IntValueReader => Factory.IdReader;

    protected override string GetErrorMessage(string request)
    {
        return $"subdivision with code {request} cannot be found";
    }
}
