using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Readers;
internal sealed class PasswordValidationReaderFactory : DatabaseReaderFactory<PasswordValidationReader>
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
internal sealed class PasswordValidationReader : SingleItemDatabaseReader<PasswordValidationReader.Request, int?>
{
    public record Request
    {
        public required string UserName { get; init; }
        public required string Password { get; init; }
    }
    public PasswordValidationReader(NpgsqlCommand command) : base(command)
    {
    }

    public IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        yield return ParameterValue.Create(PasswordValidationReaderFactory.Name, request.UserName);
        yield return ParameterValue.Create(PasswordValidationReaderFactory.Password, request.Password);
    }

    public override async Task<int?> ReadAsync(Request request)
    {
        _command.Parameters["name"].Value = request.UserName;
        _command.Parameters["password"].Value = request.Password;
        await using var reader = await _command.ExecuteReaderAsync();
        if (reader.Read()) {
            return reader.GetInt32(0);
        }
        else {
            return null;
        }
    }

}
