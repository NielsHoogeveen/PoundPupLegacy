using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Readers;

using Request = UserByEmailReaderRequest;

public sealed record UserByEmailReaderRequest : IRequest
{
    public required string Email { get; init; }
}
public sealed record UserIdByEmail
{
    public required int UserId { get; init; }
}

internal sealed class UserByEmailReaderFactory : SingleItemDatabaseReaderFactory<Request, UserIdByEmail>
{

    private static readonly NonNullableStringDatabaseParameter Email = new() { Name = "email" };

    private static readonly IntValueReader UserIdReader = new() { Name = "user_id" };

    public override string Sql => SQL;

    const string SQL = """
        select
        u.id user_id
        from "user" u
        where u.email = @email
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Email, request.Email),
        };
    }

    protected override UserIdByEmail Read(NpgsqlDataReader reader)
    {
        return new UserIdByEmail {
            UserId = UserIdReader.GetValue(reader),
        };
    }
}
