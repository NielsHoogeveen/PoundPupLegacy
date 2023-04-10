namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class UserInserterFactory : DatabaseInserterFactory<User, UserInserter>
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

    public override string TableName => "user";
}
internal sealed class UserInserter : DatabaseInserter<User>
{
    public UserInserter(NpgsqlCommand command) : base(command)
    {
    }
    public override IEnumerable<ParameterValue> GetParameterValues(User item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(UserInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(UserInserterFactory.CreatedDateTime, item.CreatedDateTime),
            ParameterValue.Create(UserInserterFactory.Email, item.Email),
            ParameterValue.Create(UserInserterFactory.Password, item.Password),
            ParameterValue.Create(UserInserterFactory.AboutMe, item.AboutMe),
            ParameterValue.Create(UserInserterFactory.AnimalWithin, item.AnimalWithin),
            ParameterValue.Create(UserInserterFactory.RelationToChildPlacement, item.RelationToChildPlacement),
            ParameterValue.Create(UserInserterFactory.Avatar, item.Avatar),
            ParameterValue.Create(UserInserterFactory.UserStatusId, item.UserStatusId),
        };
    }
}
