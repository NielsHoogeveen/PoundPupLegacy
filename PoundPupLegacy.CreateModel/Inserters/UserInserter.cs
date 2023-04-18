namespace PoundPupLegacy.CreateModel.Inserters;

using Request = User;

internal sealed class UserInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableDateTimeDatabaseParameter CreatedDateTime = new() { Name = "created_date_time" };
    internal static NullableStringDatabaseParameter AboutMe = new() { Name = "about_me" };
    internal static NullableStringDatabaseParameter AnimalWithin = new() { Name = "animal_within" };
    internal static NonNullableStringDatabaseParameter RelationToChildPlacement = new() { Name = "relation_to_child_placement" };
    internal static NonNullableStringDatabaseParameter Email = new() { Name = "email" };
    internal static NonNullableStringDatabaseParameter Password = new() { Name = "password" };
    internal static NullableStringDatabaseParameter Avatar = new() { Name = "avatar" };
    internal static NonNullableIntegerDatabaseParameter UserStatusId = new() { Name = "user_status_id" };

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
