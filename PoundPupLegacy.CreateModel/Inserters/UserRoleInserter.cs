namespace PoundPupLegacy.CreateModel.Inserters;

public class UserRoleInserterFactory : DatabaseInserterFactory<UserRole>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter UserGroupId = new() { Name = "user_group_id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override async Task<IDatabaseInserter<UserRole>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "user_role",
            new DatabaseParameter[] {
                Id,
                UserGroupId,
                Name
            }
        );
        return new UserRoleInserter(command);
    }
}
public class UserRoleInserter : DatabaseInserter<UserRole>
{

    internal UserRoleInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(UserRole userRole)
    {
        if (userRole.Id is null)
            throw new NullReferenceException();
        if(userRole.UserGroupId is null)
            throw new NullReferenceException();
        Set(UserRoleInserterFactory.Id, userRole.Id.Value);
        Set(UserRoleInserterFactory.UserGroupId, userRole.UserGroupId.Value);
        Set(UserRoleInserterFactory.Name, userRole.Name);
        await _command.ExecuteNonQueryAsync();
    }
}
