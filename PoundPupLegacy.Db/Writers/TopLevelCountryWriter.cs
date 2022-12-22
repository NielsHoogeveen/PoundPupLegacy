using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Db.Writers;

internal class TopLevelCountryWriter : DatabaseWriter<TopLevelCountry>, IDatabaseWriter<TopLevelCountry>
{
    public static DatabaseWriter<TopLevelCountry> Create(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"""INSERT INTO public."top_level_country" (id, iso_3166_1_code, global_region_id) VALUES(@id,@iso_3166_1_code,@global_region_id)""";
        command.Parameters.Add("id", NpgsqlDbType.Integer);
        command.Parameters.Add("iso_3166_1_code", NpgsqlDbType.Char);
        command.Parameters.Add("global_region_id", NpgsqlDbType.Integer);
        return new TopLevelCountryWriter(command);
    }

    internal TopLevelCountryWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(TopLevelCountry country)
    {
        _command.Parameters["id"].Value = country.Id;
        _command.Parameters["iso_3166_1_code"].Value = country.ISO3166_1_Code;
        _command.Parameters["global_region_id"].Value = country.GlobalRegionId;
        _command.ExecuteNonQuery();
    }
}
