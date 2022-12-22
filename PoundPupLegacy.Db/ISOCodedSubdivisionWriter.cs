using Npgsql;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

internal class ISOCodedSubdivisionWriter : DatabaseWriter<ISOCodedSubdivision>
{

    internal ISOCodedSubdivisionWriter(NpgsqlCommand command) : base(command)
    {
    }

    public override void Write(ISOCodedSubdivision country)
    {
        _command.Parameters["id"].Value = country.Id;
        _command.Parameters["iso_3166_2_code"].Value = country.ISO3166_2_Code;
        _command.ExecuteNonQuery();
    }
}
