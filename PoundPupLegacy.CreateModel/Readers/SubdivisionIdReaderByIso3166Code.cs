namespace PoundPupLegacy.CreateModel.Readers;
public sealed class SubdivisionIdReaderByIso3166CodeFactory : DatabaseReaderFactory<SubdivisionIdReaderByIso3166Code>
{
    internal static NonNullableStringDatabaseParameter Iso3166Code = new() { Name = "iso_3166_2_code" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT id
        FROM public.iso_coded_subdivision 
        WHERE iso_3166_2_code = @iso_3166_2_code 
        """;

}
public sealed class SubdivisionIdReaderByIso3166Code : SingleItemDatabaseReader<string, int>
{

    internal SubdivisionIdReaderByIso3166Code(NpgsqlCommand command) : base(command) { }

    public override async Task<int> ReadAsync(string code)
    {
        if (code is null) {
            throw new ArgumentNullException(nameof(code));
        }
        _command.Parameters["iso_3166_2_code"].Value = code;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var result = reader.GetInt32("id");
            await reader.CloseAsync();
            return result;
        }
        await reader.CloseAsync();
        throw new Exception($"subdivision with code {code} cannot be found");
    }
}
