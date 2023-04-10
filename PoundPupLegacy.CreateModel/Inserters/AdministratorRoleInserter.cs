namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class AdministratorRoleInserterFactory : DatabaseInserterFactory<AdministratorRole>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter UserGroupId = new() { Name = "user_group_id" };


    public override async Task<IDatabaseInserter<AdministratorRole>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "administrator_role",
            new DatabaseParameter[] {
                Id,
                UserGroupId
            }
        );
        return new AdministratorRoleInserter(command);
    }

}
internal class AdministratorRoleInserter : DatabaseInserter<AdministratorRole>
{

    internal AdministratorRoleInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(AdministratorRole administratorRole)
    {
        if (administratorRole.Id is null)
            throw new ArgumentNullException(nameof(administratorRole.Id));
        if (administratorRole.UserGroupId is null)
            throw new ArgumentNullException(nameof(administratorRole.UserGroupId));
        Set(AdministratorRoleInserterFactory.Id, administratorRole.Id.Value);
        Set(AdministratorRoleInserterFactory.UserGroupId, administratorRole.UserGroupId.Value);
        await _command.ExecuteNonQueryAsync();
    }
}
