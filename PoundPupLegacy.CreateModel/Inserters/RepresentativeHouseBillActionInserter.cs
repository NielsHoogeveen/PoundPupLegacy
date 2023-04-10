using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class RepresentativeHouseBillActionInserterFactory : DatabaseInserterFactory<RepresentativeHouseBillAction>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter RepresentativeId = new() { Name = "representative_id" };
    internal static NonNullableIntegerDatabaseParameter HouseBillId = new() { Name = "house_bill_id" };
    internal static NonNullableDateTimeDatabaseParameter Date = new() { Name = "date" };
    internal static NonNullableIntegerDatabaseParameter BillActionTypeId = new() { Name = "bill_action_type_id" };

    public override async Task<IDatabaseInserter<RepresentativeHouseBillAction>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var databaseParameters = new DatabaseParameter[] {
            RepresentativeId,
            HouseBillId,
            Date,
            BillActionTypeId
        };

        var genarateIdCommand = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "representative_house_bill_action",
            databaseParameters
        );

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "representative_house_bill_action",
            databaseParameters.ToImmutableList().Add(Id)
        );
        return new RepresentativeHouseBillActionInserter(command, genarateIdCommand);
    }
}
internal sealed class RepresentativeHouseBillActionInserter : DatabaseInserter<RepresentativeHouseBillAction>
{


    private NpgsqlCommand _generateIdCommand;

    internal RepresentativeHouseBillActionInserter(NpgsqlCommand command, NpgsqlCommand generateIdCommand) : base(command)
    {
        _generateIdCommand = generateIdCommand;
    }

    private void DoWrites(RepresentativeHouseBillAction representativeHouseBillAction, NpgsqlCommand command)
    {
        Set(RepresentativeHouseBillActionInserterFactory.RepresentativeId, representativeHouseBillAction.RepresentativeId, command);
        Set(RepresentativeHouseBillActionInserterFactory.HouseBillId, representativeHouseBillAction.HouseBillId, command);
        Set(RepresentativeHouseBillActionInserterFactory.BillActionTypeId, representativeHouseBillAction.BillActionTypeId, command);
        Set(RepresentativeHouseBillActionInserterFactory.Date, representativeHouseBillAction.Date, command);

    }

    public override async Task InsertAsync(RepresentativeHouseBillAction representativeHouseBillAction)
    {
        if (representativeHouseBillAction.Id is null) {
            DoWrites(representativeHouseBillAction, _generateIdCommand);
            representativeHouseBillAction.Id = await _command.ExecuteScalarAsync() switch {
                long i => (int)i,
                _ => throw new Exception("Insert of representative house bill action does not return an id.")
            };
        }
        else {
            Set(RepresentativeHouseBillActionInserterFactory.Id, representativeHouseBillAction.Id.Value);
            DoWrites(representativeHouseBillAction, _command);
            await _command.ExecuteNonQueryAsync();
        }
    }

    public override async ValueTask DisposeAsync()
    {
        await _generateIdCommand.DisposeAsync();
        await base.DisposeAsync();
    }

}
