using System.Data;

namespace PoundPupLegacy.Db.Readers;

public class SubdivisionIdReaderByIso3166Code : DatabaseReader<int>, IDatabaseReader<SubdivisionIdReaderByIso3166Code>
{
    public static async Task<SubdivisionIdReaderByIso3166Code> CreateAsync(NpgsqlConnection connection)
    {
        var sql = """
            SELECT id
            FROM public.iso_coded_subdivision 
            WHERE iso_3166_2_code = @iso_3166_2_code 
            """;

        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;

        command.Parameters.Add("iso_3166_2_code", NpgsqlDbType.Varchar);
        await command.PrepareAsync();

        return new SubdivisionIdReaderByIso3166Code(command);

    }

    internal SubdivisionIdReaderByIso3166Code(NpgsqlCommand command) : base(command) { }

    public async Task<int> ReadAsync(string code)
    {
        if(code is null)
        {
            throw new ArgumentNullException(nameof(code));
        }
        _command.Parameters["iso_3166_2_code"].Value = code;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows)
        {
            await reader.ReadAsync();
             var result =  reader.GetInt32("id");
            await reader.CloseAsync();
            return result;
        }
        await reader.CloseAsync();
        throw new Exception($"subdivision with code {code} cannot be found");
    }
}
