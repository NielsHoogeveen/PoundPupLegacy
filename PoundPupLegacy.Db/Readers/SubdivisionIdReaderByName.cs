using System.Data;

namespace PoundPupLegacy.Db.Readers;

public sealed class SubdivisionIdReaderByName : DatabaseUpdater<int>, IDatabaseReader<SubdivisionIdReaderByName>
{
    public static async Task<SubdivisionIdReaderByName> CreateAsync(NpgsqlConnection connection)
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

    internal SubdivisionIdReaderByName(NpgsqlCommand command) : base(command) { }

    public async Task<int> ReadAsync(int countryId, string name)
    {
        if (name is null) {
            throw new ArgumentNullException(nameof(name));
        }
        _command.Parameters["country_id"].Value = countryId;
        _command.Parameters["name"].Value = name;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var result = reader.GetInt32("id");
            await reader.CloseAsync();
            return result;
        }
        await reader.CloseAsync();
        throw new Exception($"subdivision with code {name} cannot be found");
    }
}
