namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PublishingUserGroupInserterFactory : DatabaseInserterFactory<PublishingUserGroup>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter PublicationStatusIdDefault = new() { Name = "publication_status_id_default" };

    public override async Task<IDatabaseInserter<PublishingUserGroup>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "publishing_user_group",
            new DatabaseParameter[] {
                Id,
                PublicationStatusIdDefault
            }
        );
        return new PublishingUserGroupInserter(command);
    }
}
internal sealed class PublishingUserGroupInserter : DatabaseInserter<PublishingUserGroup>
{

    internal PublishingUserGroupInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PublishingUserGroup publishingUserGroup)
    {
        if (publishingUserGroup.Id is null)
            throw new NullReferenceException();
        Set(PublishingUserGroupInserterFactory.Id, publishingUserGroup.Id.Value);
        Set(PublishingUserGroupInserterFactory.PublicationStatusIdDefault, publishingUserGroup.PublicationStatusIdDefault);
        await _command.ExecuteNonQueryAsync();
    }
}
