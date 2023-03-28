using System.Data;

namespace PoundPupLegacy.Db.Readers;
public sealed class SubdivisionIdReaderByNameFactory : IDatabaseReaderFactory<SubdivisionIdReaderByName>
{
    public async Task<SubdivisionIdReaderByName> CreateAsync(NpgsqlConnection connection)
    {
        var sql = """
            SELECT id
            FROM public.subdivision 
            WHERE country_id = @country_id
            AND name = @name 
            """;

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("country_id", NpgsqlDbType.Integer);
        command.Parameters.Add("name", NpgsqlDbType.Varchar);
        await command.PrepareAsync();

        return new SubdivisionIdReaderByName(command);

    }

}
public sealed class SubdivisionIdReaderByName : SingleItemDatabaseReader<SubdivisionIdReaderByName.SubdivisionIdReaderByNameRequest, int>
{
    public record SubdivisionIdReaderByNameRequest
    {
        public required int CountryId { get; init; }
        public required string Name { get; init; }

    }
    internal SubdivisionIdReaderByName(NpgsqlCommand command) : base(command) { }

    public override async Task<int> ReadAsync(SubdivisionIdReaderByNameRequest request)
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
