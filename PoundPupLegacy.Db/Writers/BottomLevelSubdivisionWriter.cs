namespace PoundPupLegacy.Db.Writers;

internal class BottomLevelSubdivisionWriter : IDatabaseWriter<BottomLevelSubdivision>
{
    public static DatabaseWriter<BottomLevelSubdivision> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<BottomLevelSubdivision>(SingleIdWriter.CreateSingleIdCommand("bottom_level_subdivision", connection));
    }
}
