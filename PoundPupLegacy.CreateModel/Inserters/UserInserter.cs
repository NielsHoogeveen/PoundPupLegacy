namespace PoundPupLegacy.CreateModel.Inserters;

using Request = User.ToCreate;

internal sealed class UserInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableDateTimeDatabaseParameter CreatedDateTime = new() { Name = "created_date_time" };
    private static readonly NullableStringDatabaseParameter AboutMe = new() { Name = "about_me" };
    private static readonly NullableStringDatabaseParameter AnimalWithin = new() { Name = "animal_within" };
    private static readonly NonNullableStringDatabaseParameter RelationToChildPlacement = new() { Name = "relation_to_child_placement" };
    private static readonly NonNullableStringDatabaseParameter Email = new() { Name = "email" };
    private static readonly NonNullableStringDatabaseParameter Password = new() { Name = "password" };
    private static readonly NullableStringDatabaseParameter Avatar = new() { Name = "avatar" };
    private static readonly NonNullableIntegerDatabaseParameter UserStatusId = new() { Name = "user_status_id" };

    public override string TableName => "user";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CreatedDateTime, request.CreatedDateTime),
            ParameterValue.Create(Email, request.Email),
            ParameterValue.Create(Password, request.Password),
            ParameterValue.Create(AboutMe, request.AboutMe),
            ParameterValue.Create(AnimalWithin, request.AnimalWithin),
            ParameterValue.Create(RelationToChildPlacement, request.RelationToChildPlacement),
            ParameterValue.Create(Avatar, request.Avatar),
            ParameterValue.Create(UserStatusId, request.UserStatusId),
        };
    }
}
