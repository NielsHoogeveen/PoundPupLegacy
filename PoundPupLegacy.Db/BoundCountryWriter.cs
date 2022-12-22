using Npgsql;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

internal class BoundCountryWriter : DatabaseWriter<BoundCountry>
{

    internal BoundCountryWriter(NpgsqlCommand command) : base(command)
    {
    }

    public override void Write(BoundCountry country)
    {
        _command.Parameters["id"].Value = country.Id;
        _command.Parameters["binding_country_id"].Value = country.BindingCountryId;
        _command.ExecuteNonQuery();
    }
}
