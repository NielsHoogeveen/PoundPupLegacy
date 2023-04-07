using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;

public class UserGroupInserterFactory : DatabaseInserterFactory<UserGroup>
{
    public override async Task<IDatabaseInserter<UserGroup>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var columnDefinitions = new ColumnDefinition[] {
            new ColumnDefinition
            {
                Name = UserGroupInserter.NAME,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new ColumnDefinition
            {
                Name = UserGroupInserter.DESCRIPTION,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new ColumnDefinition{
                Name = UserGroupInserter.ADMINISTRATOR_ROLE_ID,
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
                    Name = UserGroupInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                })
        );
        return new UserGroupInserter(command, identityInsertCommand);
    }
}
public class UserGroupInserter : DatabaseInserter<UserGroup>
{
    internal const string ID = "id";
    internal const string NAME = "name";
    internal const string DESCRIPTION = "description";
    internal const string ADMINISTRATOR_ROLE_ID = "administrator_role_id";


    private NpgsqlCommand _identityInsertCommand;
    internal UserGroupInserter(NpgsqlCommand command, NpgsqlCommand identityInsertCommand) : base(command)
    {
        _identityInsertCommand = identityInsertCommand;
    }

    public override async Task InsertAsync(UserGroup userGroup)
    {
        if (userGroup.Id is not null) {
            SetParameter(userGroup.Id, ID);
            SetParameter(userGroup.Name, NAME);
            SetParameter(userGroup.Description, DESCRIPTION);
            SetParameter(userGroup.AdministratorRole.Id, ADMINISTRATOR_ROLE_ID);
            await _command.ExecuteNonQueryAsync();
        }
        else {
            SetParameter(userGroup.Name, NAME, _identityInsertCommand);
            SetParameter(userGroup.Description, DESCRIPTION, _identityInsertCommand);
            SetParameter(userGroup.AdministratorRole.Id, ADMINISTRATOR_ROLE_ID);
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
