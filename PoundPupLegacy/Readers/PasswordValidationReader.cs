using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Readers;

using Factory = PasswordValidationReaderFactory;
using Reader = PasswordValidationReader;

internal sealed class PasswordValidationReaderFactory : DatabaseReaderFactory<Reader>
{
    internal static readonly NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static readonly NonNullableStringDatabaseParameter Password = new() { Name = "password" };

    public override string Sql => SQL;

    const string SQL = """
        select 
        p.id 
        from "user" u
        join publisher p on p.id = u.id
        where LOWER(p.name) = @name and u.password = @password
        """;


}
internal sealed class PasswordValidationReader : SingleItemDatabaseReader<Reader.Request, Reader.Response>
{
    public record Response
    {
    }

    public record Request
    {
        public required string UserName { get; init; }
        public required string Password { get; init; }
    }
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
        return new Response();
    }
}
