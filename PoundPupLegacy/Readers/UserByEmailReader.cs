using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;

namespace PoundPupLegacy.Readers;

using Request = UserByEmailReaderRequest;

public sealed record UserByEmailReaderRequest : IRequest
{
    public required string Email { get; init; }
}
internal sealed class UserByEmailReaderFactory : SingleItemDatabaseReaderFactory<Request, User>
{

    private static readonly NonNullableStringDatabaseParameter Email = new() { Name = "email" };

    private static readonly IntValueReader UserIdReader = new() { Name = "user_id" };
    private static readonly StringValueReader UserNameReader = new() { Name = "user_name" };

    public override string Sql => SQL;

    const string SQL = """
        select
        u.id user_id,
        p.name user_name
        from "user" u
        join publisher p on p.id = u.id
        where u.email = @email
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Email, request.Email),
        };
    }

    protected override User Read(NpgsqlDataReader reader)
    {
        return new User {
            Id = UserIdReader.GetValue(reader),
            Name = UserNameReader.GetValue(reader),
        };
    }
}
