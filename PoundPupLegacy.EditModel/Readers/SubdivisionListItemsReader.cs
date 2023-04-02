using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.EditModel.Readers;
public class SubdivisionListItemsReaderFactory : IDatabaseReaderFactory<SubdivisionListItemsReader>
{
    public async Task<SubdivisionListItemsReader> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();

        var sql = $"""
            select
                s.id,
                s.name
                from subdivision s
                join bottom_level_subdivision bls on bls.id = s.id
                where s.country_id = @country_id
                order by s.name
            """;
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;
        command.Parameters.Add("country_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await command.PrepareAsync();
        return new SubdivisionListItemsReader(command);

    }
}
public class SubdivisionListItemsReader : DatabaseReader, IEnumerableDatabaseReader<int, SubdivisionListItem>
{
    internal SubdivisionListItemsReader(NpgsqlCommand command) : base(command)
    {
    }
    public async IAsyncEnumerable<SubdivisionListItem> ReadAsync(int countryId)
    {
        _command.Parameters["country_id"].Value = countryId;
        await using var reader = await _command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);
            yield return new SubdivisionListItem {
                Id = id,
                Name = name
            };
        }
    }
}
