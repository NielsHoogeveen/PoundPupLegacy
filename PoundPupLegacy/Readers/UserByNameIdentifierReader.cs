using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;

namespace PoundPupLegacy.Readers;

using Request = UserByNameIdentifierReaderRequest;

public sealed record UserByNameIdentifierReaderRequest : IRequest
{
    public required string NameIdentifier { get; init; }
}
internal sealed class UserByNameIdentifierReaderFactory : SingleItemDatabaseReaderFactory<Request, User>
{

    private static readonly NonNullableStringDatabaseParameter NameIdentifier = new() { Name = "name_identifier" };

    private static readonly IntValueReader UserIdReader = new() { Name = "user_id" };
    private static readonly StringValueReader UserNameReader = new() { Name = "user_name" };

    public override string Sql => SQL;

    const string SQL = """
        select
        u.id user_id,
        p.name user_name
        from "user" u
        join publisher p on p.id = u.id
        where u.name_identifier = @name_identifier
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NameIdentifier, request.NameIdentifier),
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
