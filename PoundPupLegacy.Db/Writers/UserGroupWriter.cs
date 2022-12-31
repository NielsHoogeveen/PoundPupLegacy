namespace PoundPupLegacy.Db.Writers;

internal class UserGroupWriter : IDatabaseWriter<UserGroup>
{
    public static DatabaseWriter<UserGroup> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<UserGroup>(SingleIdWriter.CreateSingleIdCommand("user_group", connection));
    }
}
