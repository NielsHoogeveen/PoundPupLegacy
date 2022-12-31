namespace PoundPupLegacy.Db.Writers;

internal class FamilySizeWriter : IDatabaseWriter<FamilySize>
{
    public static DatabaseWriter<FamilySize> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<FamilySize>(SingleIdWriter.CreateSingleIdCommand("family_size", connection));
    }
}
