using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;
public class UserGroupInserter : DatabaseInserter<UserGroup>, IDatabaseInserter<UserGroup>
{
    private const string ID = "id";
    private const string NAME = "name";
    private const string DESCRIPTION = "description";
    private const string ADMINISTRATOR_ROLE_ID = "administrator_role_id";

    public static async Task<DatabaseInserter<UserGroup>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var columnDefinitions = new ColumnDefinition[] {
            new ColumnDefinition
            {
                Name = NAME,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new ColumnDefinition
            {
                Name = DESCRIPTION,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new ColumnDefinition{
                Name = ADMINISTRATOR_ROLE_ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },

        };

        var identityInsertCommand = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "user_group",
            columnDefinitions
        );
        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "user_group",
            columnDefinitions.ToImmutableList().Add(
                new ColumnDefinition {
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                })

        );
        return new UserGroupInserter(command, identityInsertCommand);
    }

    private NpgsqlCommand _identityInsertCommand;
    private UserGroupInserter(NpgsqlCommand command, NpgsqlCommand identityInsertCommand) : base(command)
    {
        _identityInsertCommand = identityInsertCommand;
    }

    public override async Task InsertAsync(UserGroup userGroup)
    {
        if (userGroup.Id is not null) {
            WriteValue(userGroup.Id, ID);
            WriteValue(userGroup.Name, NAME);
            WriteValue(userGroup.Description, DESCRIPTION);
            WriteValue(userGroup.AdministratorRole.Id, ADMINISTRATOR_ROLE_ID);
            await _command.ExecuteNonQueryAsync();
        }
        else {
            WriteValue(userGroup.Name, NAME, _identityInsertCommand);
            WriteValue(userGroup.Description, DESCRIPTION, _identityInsertCommand);
            WriteValue(userGroup.AdministratorRole.Id, ADMINISTRATOR_ROLE_ID);
            userGroup.Id = await _command.ExecuteScalarAsync() switch {
                int i => i,
                _ => throw new Exception("Insert of userGroup does not return an id.")
            };
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await _identityInsertCommand.DisposeAsync();
        await base.DisposeAsync();
    }
}
