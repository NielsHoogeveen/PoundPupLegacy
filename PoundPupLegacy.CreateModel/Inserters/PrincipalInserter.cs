﻿using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;
public class PrincipalInserter : DatabaseInserter<Principal>, IDatabaseInserter<Principal>
{
    private const string ID = "id";

    public static async Task<DatabaseInserter<Principal>> CreateAsync(NpgsqlConnection connection)
    {
        var columnDefinitions = new ColumnDefinition[] {
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
                new ColumnDefinition {
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                })

        );
        return new PrincipalInserter(command, identityInsertCommand);
    }

    private NpgsqlCommand _identityInsertCommand;
    private PrincipalInserter(NpgsqlCommand command, NpgsqlCommand identityInsertCommand) : base(command)
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