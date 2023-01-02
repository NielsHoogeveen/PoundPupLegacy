using System.Collections.Immutable;

namespace PoundPupLegacy.Db.Writers;

internal class AccessRoleWriter : DatabaseWriter<AccessRole>, IDatabaseWriter<AccessRole>
{
    private const string ID = "id";
    private const string NAME = "name";
    public static DatabaseWriter<AccessRole> Create(NpgsqlConnection connection)
    {
        var columnDefinitions = new ColumnDefinition[] {
            new ColumnDefinition{
                Name = NAME,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
        };
        var commandWithId = CreateInsertStatement(
            connection,
            "access_role",
            columnDefinitions.ToImmutableList().Prepend(new ColumnDefinition
            {
                Name = ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            })
        );
        var commandWithoutId = CreateInsertStatement(
            connection,
            "access_role",
            columnDefinitions
        );
        return new AccessRoleWriter(commandWithId, commandWithoutId);

    }

    private NpgsqlCommand _idenityCommand;
    internal AccessRoleWriter(NpgsqlCommand command, NpgsqlCommand idenityCommand) : base(command)
    {
        _idenityCommand = idenityCommand;
    }

    internal override void Write(AccessRole accessRole)
    {
        if (accessRole.Id is null)
        {
            WriteValue(accessRole.Name, NAME, _idenityCommand);
            accessRole.Id = _idenityCommand.ExecuteScalar() switch
            {
                int i => i,
                _ => throw new Exception("Id could not be set for access role")
            };
        }
        else
        {
            WriteValue(accessRole.Id, ID);
            WriteValue(accessRole.Name, NAME);
            _command.ExecuteNonQuery();
        }
    }
    public override void Dispose()
    {
        base.Dispose();
        _idenityCommand.Dispose();
    }
}
