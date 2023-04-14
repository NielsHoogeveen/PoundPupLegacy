namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = UserInserterFactory;
using Request = User;
using Inserter = UserInserter;
internal sealed class UserInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
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
}
internal sealed class UserInserter : IdentifiableDatabaseInserter<Request>
{
    public UserInserter(NpgsqlCommand command) : base(command)
    {
    }
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.CreatedDateTime, request.CreatedDateTime),
            ParameterValue.Create(Factory.Email, request.Email),
            ParameterValue.Create(Factory.Password, request.Password),
            ParameterValue.Create(Factory.AboutMe, request.AboutMe),
            ParameterValue.Create(Factory.AnimalWithin, request.AnimalWithin),
            ParameterValue.Create(Factory.RelationToChildPlacement, request.RelationToChildPlacement),
            ParameterValue.Create(Factory.Avatar, request.Avatar),
            ParameterValue.Create(Factory.UserStatusId, request.UserStatusId),
        };
    }
}
