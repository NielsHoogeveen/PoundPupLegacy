namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class UserInserterFactory : DatabaseInserterFactory<User>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableDateTimeDatabaseParameter CreatedDateTime = new() { Name = "created_date_time" };
    internal static NullableStringDatabaseParameter AboutMe = new() { Name = "about_me" };
    internal static NullableStringDatabaseParameter AnimalWithin = new() { Name = "animal_within" };
    internal static NonNullableStringDatabaseParameter RelationToChildPlacement = new() { Name = "relation_to_child_placement" };
    internal static NonNullableStringDatabaseParameter Email = new() { Name = "email" };
    internal static NonNullableStringDatabaseParameter Password = new() { Name = "password" };
    internal static NullableStringDatabaseParameter Avatar = new() { Name = "avatar" };
    internal static NonNullableIntegerDatabaseParameter UserStatusId = new() { Name = "user_status_id" };

    public override async Task<IDatabaseInserter<User>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "user",
            new DatabaseParameter[] {
                Id,
                CreatedDateTime,
                AboutMe,
                AnimalWithin,
                RelationToChildPlacement,
                Email,
                Password,
                Avatar,
                UserStatusId
            }
        );
        return new UserInserter(command);
    }
}
internal sealed class UserInserter : DatabaseInserter<User>
{
    internal UserInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(User user)
    {
        if (user.Id is null)
            throw new NullReferenceException();
        Set(UserInserterFactory.Id, user.Id.Value);
        Set(UserInserterFactory.CreatedDateTime, user.CreatedDateTime);
        Set(UserInserterFactory.Email, user.Email);
        Set(UserInserterFactory.Password, user.Password);
        Set(UserInserterFactory.AboutMe, user.AboutMe);
        Set(UserInserterFactory.AnimalWithin, user.AnimalWithin);
        Set(UserInserterFactory.RelationToChildPlacement, user.RelationToChildPlacement);
        Set(UserInserterFactory.Avatar, user.Avatar);
        Set(UserInserterFactory.UserStatusId, user.UserStatusId);
        await _command.ExecuteNonQueryAsync();
    }
}
