namespace PoundPupLegacy.Db.Writers;

internal class HagueStatusWriter : IDatabaseWriter<HagueStatus>
{
    public static DatabaseWriter<HagueStatus> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<HagueStatus>(SingleIdWriter.CreateSingleIdCommand("hague_status", connection));
    }
}
