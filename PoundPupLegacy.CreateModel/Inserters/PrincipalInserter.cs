using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;

public class PrincipalInserterFactory : DatabaseInserterFactory<Principal>
{
    public override async Task<IDatabaseInserter<Principal>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var columnDefinitions = new ColumnDefinition[] {
        };

        var identityInsertCommand = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "principal",
            columnDefinitions
        );
        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "principal",
            columnDefinitions.ToImmutableList().Add(
                new ColumnDefinition {
                    Name = PrincipalInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                })

        );
        return new PrincipalInserter(command, identityInsertCommand);
    }
}
public class PrincipalInserter : DatabaseInserter<Principal>
{
    internal const string ID = "id";


    private NpgsqlCommand _identityInsertCommand;
    internal PrincipalInserter(NpgsqlCommand command, NpgsqlCommand identityInsertCommand) : base(command)
    {
        _identityInsertCommand = identityInsertCommand;
    }

    public override async Task InsertAsync(Principal principal)
    {
        if (principal.Id is not null) {
            WriteValue(principal.Id, ID);
            await _command.ExecuteNonQueryAsync();
        }
        else {
            principal.Id = await _command.ExecuteScalarAsync() switch {
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
