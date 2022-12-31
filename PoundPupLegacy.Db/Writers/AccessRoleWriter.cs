namespace PoundPupLegacy.Db.Writers;

internal class AccessRoleWriter : DatabaseWriter<AccessRole>, IDatabaseWriter<AccessRole>
{
    private const string ID = "id";
    private const string NAME = "name";
    public static DatabaseWriter<AccessRole> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "access_role",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new AccessRoleWriter(command);

    }

    internal AccessRoleWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(AccessRole accessRole)
    {
        try
        {
            WriteValue(accessRole.Id, ID);
            WriteValue(accessRole.Name, NAME);
            _command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
