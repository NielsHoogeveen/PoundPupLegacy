namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class UserInserterFactory : DatabaseInserterFactory<User>
{
    public override async Task<IDatabaseInserter<User>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "user",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = UserInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = UserInserter.CREATED_DATE_TIME,
                    NpgsqlDbType = NpgsqlDbType.Timestamp
                },
                new ColumnDefinition{
                    Name = UserInserter.ABOUT_ME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = UserInserter.ANIMAL_WITHIN,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = UserInserter.RELATION_TO_CHILD_PLACEMENT,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = UserInserter.EMAIL,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = UserInserter.PASSWORD,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = UserInserter.AVATAR,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = UserInserter.USER_STATUS_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new UserInserter(command);
    }
}
internal sealed class UserInserter : DatabaseInserter<User>
{
    internal const string ID = "id";
    internal const string CREATED_DATE_TIME = "created_date_time";
    internal const string ABOUT_ME = "about_me";
    internal const string ANIMAL_WITHIN = "animal_within";
    internal const string RELATION_TO_CHILD_PLACEMENT = "relation_to_child_placement";
    internal const string EMAIL = "email";
    internal const string PASSWORD = "password";
    internal const string AVATAR = "avatar";
    internal const string USER_STATUS_ID = "user_status_id";

    internal UserInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(User user)
    {
        if (user.Id is null)
            throw new NullReferenceException();
        WriteValue(user.Id, ID);
        WriteValue(user.CreatedDateTime, CREATED_DATE_TIME);
        WriteValue(user.Email, EMAIL);
        WriteValue(user.Password, PASSWORD);
        WriteNullableValue(user.AboutMe, ABOUT_ME);
        WriteNullableValue(user.AnimalWithin, ANIMAL_WITHIN);
        WriteNullableValue(user.RelationToChildPlacement, RELATION_TO_CHILD_PLACEMENT);
        WriteNullableValue(user.Avatar, AVATAR);
        WriteValue(user.UserStatusId, USER_STATUS_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
