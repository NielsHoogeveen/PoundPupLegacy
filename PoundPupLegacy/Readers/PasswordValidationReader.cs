using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Readers;

using Request = PasswordValidationReaderRequest;
using Response = PasswordValidationReaderResponse;

public record PasswordValidationReaderResponse
{
    public required int UserId { get; init; }
}


public sealed record PasswordValidationReaderRequest : IRequest
{
    public required string UserName { get; init; }
    public required string Password { get; init; }
}

internal sealed class PasswordValidationReaderFactory : SingleItemDatabaseReaderFactory<Request, Response>
{
    private static readonly NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    private static readonly NonNullableStringDatabaseParameter Password = new() { Name = "password" };

    private static readonly IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        select 
        p.id 
        from "user" u
        join publisher p on p.id = u.id
        where LOWER(p.name) = @name and u.password = @password
        """;


    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        yield return ParameterValue.Create(Name, request.UserName);
        yield return ParameterValue.Create(Password, request.Password);
    }

    protected override Response Read(NpgsqlDataReader reader)
    {
        return new Response { UserId = IdReader.GetValue(reader) };
    }
}
