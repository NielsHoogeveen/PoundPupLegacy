namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterCountryRelationTypeInserterFactory : DatabaseInserterFactory<InterCountryRelationType>
{
    public override async Task<IDatabaseInserter<InterCountryRelationType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "inter_country_relation_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = InterCountryRelationTypeInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = InterCountryRelationTypeInserter.IS_SYMMETRIC,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
            }
        );
        return new InterCountryRelationTypeInserter(command);
    }

}
internal sealed class InterCountryRelationTypeInserter : DatabaseInserter<InterCountryRelationType>
{
    internal const string ID = "id";
    internal const string IS_SYMMETRIC = "is_symmetric";


    internal InterCountryRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(InterCountryRelationType interCountryRelationType)
    {
        if (interCountryRelationType.Id is null)
            throw new NullReferenceException();
        SetParameter(interCountryRelationType.Id, ID);
        SetParameter(interCountryRelationType.IsSymmetric, IS_SYMMETRIC);
        await _command.ExecuteNonQueryAsync();
    }

}
