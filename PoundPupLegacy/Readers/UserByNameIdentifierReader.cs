using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Readers;

using Request = UserByNameIdentifierReaderRequest;

public sealed record UserByNameIdentifierReaderRequest : IRequest
{
    public required string NameIdentifier { get; init; }
}
public sealed record UserIdByNameIdentifier
{
    public required int UserId { get; init; }
}

internal sealed class UserByNameIdentifierReaderFactory : SingleItemDatabaseReaderFactory<Request, UserIdByNameIdentifier>
{

    private static readonly NonNullableStringDatabaseParameter NameIdentifier = new() { Name = "name_identifier" };

    private static readonly IntValueReader UserIdReader = new() { Name = "user_id" };

    public override string Sql => SQL;

    const string SQL = """
        select
        u.id user_id
        from "user" u
        where u.name_identifier = @name_identifier
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NameIdentifier, request.NameIdentifier),
        };
    }

    protected override UserIdByNameIdentifier Read(NpgsqlDataReader reader)
    {
        return new UserIdByNameIdentifier {
            UserId = UserIdReader.GetValue(reader),
        };
    }
}
