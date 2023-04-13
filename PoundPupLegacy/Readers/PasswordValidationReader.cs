using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Readers;

using Request = PasswordValidationReaderRequest;
using Response = PasswordValidationReaderResponse;
using Factory = PasswordValidationReaderFactory;
using Reader = PasswordValidationReader;

public record PasswordValidationReaderResponse
{
    public required int UserId { get; init; }
}


public sealed record PasswordValidationReaderRequest : IRequest
{
    public required string UserName { get; init; }
    public required string Password { get; init; }
}

internal sealed class PasswordValidationReaderFactory : SingleItemDatabaseReaderFactory<Request, Response, Reader>
{
    internal static readonly NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static readonly NonNullableStringDatabaseParameter Password = new() { Name = "password" };

    internal static readonly IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        select 
        p.id 
        from "user" u
        join publisher p on p.id = u.id
        where LOWER(p.name) = @name and u.password = @password
        """;


}
internal sealed class PasswordValidationReader : SingleItemDatabaseReader<Request, Response>
{

    public PasswordValidationReader(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        yield return ParameterValue.Create(Factory.Name, request.UserName);
        yield return ParameterValue.Create(Factory.Password, request.Password);
    }

    protected override Response Read(NpgsqlDataReader reader)
    {
        return new Response { UserId = Factory.IdReader.GetValue(reader) };
    }
}
