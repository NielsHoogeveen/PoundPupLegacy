using System.Collections.Immutable;

namespace PoundPupLegacy.Db.Writers;

internal class AccessRoleWriter : DatabaseWriter<AccessRole>, IDatabaseWriter<AccessRole>
{
    private const string ID = "id";
    private const string NAME = "name";
    public static async Task<DatabaseWriter<AccessRole>> CreateAsync(NpgsqlConnection connection)
    {
        var columnDefinitions = new ColumnDefinition[] {
            new ColumnDefinition{
                Name = NAME,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
        };
        var commandWithId = await CreateInsertStatementAsync(
            connection,
            "access_role",
            columnDefinitions.ToImmutableList().Prepend(new ColumnDefinition
            {
                Name = ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            })
        );
        var commandWithoutId = await CreateInsertStatementAsync(
            connection,
            "access_role",
            columnDefinitions
        );
        return new AccessRoleWriter(commandWithId, commandWithoutId);

    }

    private NpgsqlCommand _identityCommand;
    internal AccessRoleWriter(NpgsqlCommand command, NpgsqlCommand idenityCommand) : base(command)
    {
        _identityCommand = idenityCommand;
    }

    internal override async Task WriteAsync(AccessRole accessRole)
    {
        if (accessRole.Id is null)
        {
            WriteValue(accessRole.Name, NAME, _identityCommand);
            accessRole.Id = await _identityCommand.ExecuteScalarAsync() switch
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
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _identityCommand.DisposeAsync();

    }
}
