using Npgsql;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

internal class SubdivisionWriter : DatabaseWriter<Subdivision>
{

    internal SubdivisionWriter(NpgsqlCommand command) : base(command)
    {
    }

    public override void Write(Subdivision subdivision)
    {
        _command.Parameters["id"].Value = subdivision.Id;
        _command.Parameters["name"].Value = subdivision.Name;
        _command.Parameters["country_id"].Value = subdivision.CountryId;
        _command.ExecuteNonQuery();
    }
}
