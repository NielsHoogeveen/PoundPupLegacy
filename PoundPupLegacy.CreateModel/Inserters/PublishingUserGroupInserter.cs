namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PublishingUserGroupInserter : DatabaseInserter<PublishingUserGroup>, IDatabaseInserter<PublishingUserGroup>
{

    private const string ID = "id";
    private const string PUBLICATION_STATUS_ID_DEFAULT = "publication_status_id_default";
    public static async Task<DatabaseInserter<PublishingUserGroup>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "publishing_user_group",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PUBLICATION_STATUS_ID_DEFAULT,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new PublishingUserGroupInserter(command);

    }

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
