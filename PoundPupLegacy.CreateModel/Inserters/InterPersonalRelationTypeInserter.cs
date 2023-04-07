namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterPersonalRelationTypeInserterFactory : DatabaseInserterFactory<InterPersonalRelationType>
{
    public override async Task<IDatabaseInserter<InterPersonalRelationType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "inter_personal_relation_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = InterPersonalRelationTypeInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = InterPersonalRelationTypeInserter.IS_SYMMETRIC,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
            }
        );
        return new InterPersonalRelationTypeInserter(command);
    }

}
internal sealed class InterPersonalRelationTypeInserter : DatabaseInserter<InterPersonalRelationType>
{
    internal const string ID = "id";
    internal const string IS_SYMMETRIC = "is_symmetric";


    internal InterPersonalRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(InterPersonalRelationType interPersonalRelationType)
    {
        if (interPersonalRelationType.Id is null)
            throw new NullReferenceException();
        SetParameter(interPersonalRelationType.Id, ID);
        SetParameter(interPersonalRelationType.IsSymmetric, IS_SYMMETRIC);
        await _command.ExecuteNonQueryAsync();
    }

}
