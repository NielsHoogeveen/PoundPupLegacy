namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PublishingUserGroupInserterFactory : DatabaseInserterFactory<PublishingUserGroup>
{
    public override async Task<IDatabaseInserter<PublishingUserGroup>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "publishing_user_group",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = PublishingUserGroupInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PublishingUserGroupInserter.PUBLICATION_STATUS_ID_DEFAULT,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new PublishingUserGroupInserter(command);
    }
}
internal sealed class PublishingUserGroupInserter : DatabaseInserter<PublishingUserGroup>
{

    internal const string ID = "id";
    internal const string PUBLICATION_STATUS_ID_DEFAULT = "publication_status_id_default";

    internal PublishingUserGroupInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PublishingUserGroup publishingUserGroup)
    {
        WriteValue(publishingUserGroup.Id, ID);
        WriteNullableValue(publishingUserGroup.PublicationStatusIdDefault, PUBLICATION_STATUS_ID_DEFAULT);
        await _command.ExecuteNonQueryAsync();
    }
}
