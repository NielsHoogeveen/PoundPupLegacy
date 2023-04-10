using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;

public class UserGroupInserterFactory : DatabaseInserterFactory<UserGroup>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    internal static NonNullableIntegerDatabaseParameter AdministratorRoleId = new() { Name = "administrator_role_id" };

    public override async Task<IDatabaseInserter<UserGroup>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var databaseParameters = new DatabaseParameter[] {
            Name,
            Description,
            AdministratorRoleId
        };

        var identityInsertCommand = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "user_group",
            databaseParameters
        );
        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "user_group",
            databaseParameters.ToImmutableList().Add(Id)
        );
        return new UserGroupInserter(command, identityInsertCommand);
    }
}
public class UserGroupInserter : DatabaseInserter<UserGroup>
{
    private NpgsqlCommand _identityInsertCommand;
    internal UserGroupInserter(NpgsqlCommand command, NpgsqlCommand identityInsertCommand) : base(command)
    {
        _identityInsertCommand = identityInsertCommand;
    }

    public override async Task InsertAsync(UserGroup userGroup)
    {
        if (userGroup.AdministratorRole.Id is null)
            throw new ArgumentNullException(nameof(userGroup.AdministratorRole.Id));
        if (userGroup.Id is not null) {
            Set(UserGroupInserterFactory.Id,userGroup.Id.Value);
            Set(UserGroupInserterFactory.Name, userGroup.Name);
            Set(UserGroupInserterFactory.Description, userGroup.Description);
            Set(UserGroupInserterFactory.AdministratorRoleId, userGroup.AdministratorRole.Id.Value);
            await _command.ExecuteNonQueryAsync();
        }
        else {
            Set(UserGroupInserterFactory.Name, userGroup.Name);
            Set(UserGroupInserterFactory.Description, userGroup.Description);
            Set(UserGroupInserterFactory.AdministratorRoleId, userGroup.AdministratorRole.Id.Value);
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
