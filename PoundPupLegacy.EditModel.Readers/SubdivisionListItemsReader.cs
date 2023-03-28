using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.EditModel.Readers;

public class SubdivisionListItemsReader : DatabaseReader, ISingleItemDatabaseReader<SubdivisionListItemsReader, int, List<SubdivisionListItem>>
{
    private SubdivisionListItemsReader(NpgsqlCommand command) : base(command)
    {
    }
    public async Task<List<SubdivisionListItem>> ReadAsync(int countryId)
    {
        _command.Parameters["country_id"].Value = countryId;
        await using var reader = await _command.ExecuteReaderAsync();
        await reader.ReadAsync();
        if (!reader.HasRows) {
            return new List<SubdivisionListItem>();
        }
        var text = reader.GetString(0); ;
        var subdivisions = reader.GetFieldValue<List<SubdivisionListItem>>(0);
        return subdivisions;

    }
    public static async Task<SubdivisionListItemsReader> CreateAsync(NpgsqlConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = SQL;
        command.Parameters.Add("country_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await command.PrepareAsync();
        return new SubdivisionListItemsReader(command);

    }

    const string SQL = """
        subdivisions_document as(
            select
            c.country_id,
            jsonb_agg(
        	    jsonb_build_object(
        		    'Id',
        		    c.id,
        		    'Name',
        		    t.name
        	    )
            ) document
            from subdivision c
            join bottom_level_subdivision b on b.id = c.id
            join term t on t.nameable_id = c.id
            join tenant_node tn on tn.node_id = t.vocabulary_id
            where tn.tenant_id = 1 and tn.url_id = 4126
            group by c.country_id
        )
        """;

}
