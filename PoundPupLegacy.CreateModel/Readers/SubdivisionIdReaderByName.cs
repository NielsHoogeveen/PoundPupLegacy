namespace PoundPupLegacy.CreateModel.Readers;
public sealed class SubdivisionIdReaderByNameFactory : DatabaseReaderFactory<SubdivisionIdReaderByName>
{
    internal static NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT id
        FROM public.subdivision 
        WHERE country_id = @country_id
        AND name = @name 
        """;

}
public sealed class SubdivisionIdReaderByName : SingleItemDatabaseReader<SubdivisionIdReaderByName.Request, int>
{
    public record Request
    {
        public required int CountryId { get; init; }
        public required string Name { get; init; }

    }
    internal SubdivisionIdReaderByName(NpgsqlCommand command) : base(command) { }

    public override async Task<int> ReadAsync(Request request)
    {
        if (request.Name is null) {
            throw new ArgumentNullException(nameof(request.Name));
        }
        _command.Parameters["country_id"].Value = request.CountryId;
        _command.Parameters["name"].Value = request.Name;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var result = reader.GetInt32("id");
            await reader.CloseAsync();
            return result;
        }
        await reader.CloseAsync();
        throw new Exception($"subdivision with code {request.Name} cannot be found");
    }
}
