namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterCountryRelationTypeInserterFactory : DatabaseInserterFactory<InterCountryRelationType>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableBooleanDatabaseParameter IsSymmetric = new() { Name = "is_symmetric" };

    public override async Task<IDatabaseInserter<InterCountryRelationType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "inter_country_relation_type",
            new DatabaseParameter[] {
                Id,
                IsSymmetric
            }
        );
        return new InterCountryRelationTypeInserter(command);
    }

}
internal sealed class InterCountryRelationTypeInserter : DatabaseInserter<InterCountryRelationType>
{
    internal InterCountryRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(InterCountryRelationType interCountryRelationType)
    {
        if (interCountryRelationType.Id is null)
            throw new NullReferenceException();
        Set(InterCountryRelationTypeInserterFactory.Id, interCountryRelationType.Id.Value);
        Set(InterCountryRelationTypeInserterFactory.IsSymmetric, interCountryRelationType.IsSymmetric);
        await _command.ExecuteNonQueryAsync();
    }

}
