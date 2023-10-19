namespace PoundPupLegacy.Updaters;

using PoundPupLegacy.Common;
using Request = UserNameIdentifierUpdaterRequest;

public record UserNameIdentifierUpdaterRequest: IRequest
{
    public required int UserId { get; init; }
    public required string NameIdentifier { get; init; }
}
internal sealed class UserNameIdentifierUpdaterFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };
    private static readonly NonNullableStringDatabaseParameter NameIdentifier = new() { Name = "name_identifier" };

    public override string Sql => $"""
        update "user"
        set name_identifier = @name_identifier
        where id = @user_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(UserId, request.UserId),
            ParameterValue.Create(NameIdentifier, request.NameIdentifier),
          
        };
    }
}

