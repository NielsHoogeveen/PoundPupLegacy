using Npgsql;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

internal class TopLevelCountryWriter : DatabaseWriter<TopLevelCountry>
{

    internal TopLevelCountryWriter(NpgsqlCommand command) : base(command)
    {
    }

    public override void Write(TopLevelCountry country)
    {
        _command.Parameters["id"].Value = country.Id;
        _command.Parameters["iso_3166_1_code"].Value = country.ISO3166_1_Code;
        _command.Parameters["global_region_id"].Value = country.GlobalRegionId;
        _command.ExecuteNonQuery();
    }
}
