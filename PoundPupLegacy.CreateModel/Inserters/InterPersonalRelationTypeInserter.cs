namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterPersonalRelationTypeInserterFactory : DatabaseInserterFactory<InterPersonalRelationType>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableBooleanDatabaseParameter IsSymmetric = new() { Name = "is_symmetric" };

    public override async Task<IDatabaseInserter<InterPersonalRelationType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "inter_personal_relation_type",
            new DatabaseParameter[] {
                Id,
                IsSymmetric
            }
        );
        return new InterPersonalRelationTypeInserter(command);
    }

}
internal sealed class InterPersonalRelationTypeInserter : DatabaseInserter<InterPersonalRelationType>
{


    internal InterPersonalRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(InterPersonalRelationType interPersonalRelationType)
    {
        if (interPersonalRelationType.Id is null)
            throw new NullReferenceException();
        Set(InterPersonalRelationTypeInserterFactory.Id, interPersonalRelationType.Id.Value);
        Set(InterPersonalRelationTypeInserterFactory.IsSymmetric,interPersonalRelationType.IsSymmetric);
        await _command.ExecuteNonQueryAsync();
    }

}
