namespace PoundPupLegacy.CreateModel.Inserters;
public class UnitedStatesPoliticalPartyInserter : IDatabaseInserter<UnitedStatesPoliticalParty>
{
    public static async Task<DatabaseInserter<UnitedStatesPoliticalParty>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<UnitedStatesPoliticalParty>("united_states_political_party", connection);
    }
}
