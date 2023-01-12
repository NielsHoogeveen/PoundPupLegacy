using System.Collections.Immutable;

namespace PoundPupLegacy.Db.Writers;
public class PrincipalWriter : DatabaseWriter<Principal>, IDatabaseWriter<Principal>
{
    private const string ID = "id";
    private const string NAME = "name";

    public static async Task<DatabaseWriter<Principal>> CreateAsync(NpgsqlConnection connection)
    {
        var columnDefinitions = new ColumnDefinition[] {
            new ColumnDefinition
            {
                Name = NAME,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
        };

        var identityInsertCommand = await CreateIdentityInsertStatementAsync(
            connection,
            "principal",
            columnDefinitions
        );
        var command = await CreateInsertStatementAsync(
            connection,
            "principal",
            columnDefinitions.ToImmutableList().Add(
                new ColumnDefinition 
                { 
                    Name = ID, 
                    NpgsqlDbType = NpgsqlDbType.Integer
                })

        );
        return new PrincipalWriter(command, identityInsertCommand);
    }

    private NpgsqlCommand _identityInsertCommand;
    private PrincipalWriter(NpgsqlCommand command, NpgsqlCommand identityInsertCommand) : base(command)
    {
        _identityInsertCommand = identityInsertCommand;
    }

    internal override async Task WriteAsync(Principal principal)
    {
        if(principal.Id is not null)
        {
            WriteValue(principal.Id, ID);
            WriteValue(principal.Name, NAME);
            await _command.ExecuteNonQueryAsync();
        }
        else
        {
            WriteValue(principal.Name, NAME, _identityInsertCommand);
            principal.Id = await _command.ExecuteScalarAsync() switch
            {
                int i => i,
                _ => throw new Exception("Insert of principal does not return an id.")
            };
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await _identityInsertCommand.DisposeAsync();
        await base.DisposeAsync();
    }
}
