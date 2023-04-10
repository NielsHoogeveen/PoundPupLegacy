namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterPersonalRelationInserterFactory : DatabaseInserterFactory<InterPersonalRelation>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter PersonIdFrom = new() { Name = "person_id_from" };
    internal static NonNullableIntegerDatabaseParameter PersonIdTo = new() { Name = "person_id_to" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NonNullableIntegerDatabaseParameter InterPersonalRelationTypeId = new() { Name = "inter_personal_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };

    public override async Task<IDatabaseInserter<InterPersonalRelation>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "inter_personal_relation",
            new DatabaseParameter[] {
                Id,
                PersonIdFrom,
                PersonIdTo,
                DateRange,
                InterPersonalRelationTypeId,
                DocumentIdProof
            }
        );
        return new InterPersonalRelationInserter(command);

    }
}
internal sealed class InterPersonalRelationInserter : DatabaseInserter<InterPersonalRelation>
{
    internal InterPersonalRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(InterPersonalRelation interPersonalRelation)
    {
        if (interPersonalRelation.Id is null)
            throw new NullReferenceException();
        Set(InterPersonalRelationInserterFactory.Id, interPersonalRelation.Id.Value);
        Set(InterPersonalRelationInserterFactory.PersonIdFrom, interPersonalRelation.PersonIdFrom);
        Set(InterPersonalRelationInserterFactory.PersonIdTo, interPersonalRelation.PersonIdTo);
        Set(InterPersonalRelationInserterFactory.InterPersonalRelationTypeId, interPersonalRelation.InterPersonalRelationTypeId);
        Set(InterPersonalRelationInserterFactory.DateRange, interPersonalRelation.DateRange);
        Set(InterPersonalRelationInserterFactory.DocumentIdProof, interPersonalRelation.DocumentIdProof);
        await _command.ExecuteNonQueryAsync();
    }
}
