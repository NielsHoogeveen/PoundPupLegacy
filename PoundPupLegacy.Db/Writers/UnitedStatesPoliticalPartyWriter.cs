using Action = PoundPupLegacy.Model.Action;

namespace PoundPupLegacy.Db.Writers;
public class UnitedStatesPoliticalPartyWriter : IDatabaseWriter<UnitedStatesPoliticalParty>
{
    public static async Task<DatabaseWriter<UnitedStatesPoliticalParty>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<UnitedStatesPoliticalParty>("united_states_political_party", connection);
    }
}
