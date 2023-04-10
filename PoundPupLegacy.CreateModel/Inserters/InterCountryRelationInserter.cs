namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterCountryRelationInserterFactory : DatabaseInserterFactory<InterCountryRelation>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter CountryIdFrom = new() { Name = "country_id_from" };
    internal static NonNullableIntegerDatabaseParameter CountryIdTo = new() { Name = "country_id_to" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NullableIntegerDatabaseParameter NumberOfChildrenInvolved = new() { Name = "number_of_children_involved" };
    internal static NullableDecimalDatabaseParameter MoneyInvolved = new() { Name = "money_involved" };
    internal static NonNullableIntegerDatabaseParameter InterCountryRelationTypeId = new() { Name = "inter_country_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };

    public override async Task<IDatabaseInserter<InterCountryRelation>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "inter_country_relation",
            new DatabaseParameter[] {
                Id,
                CountryIdFrom,
                CountryIdTo,
                DateRange,
                NumberOfChildrenInvolved,
                MoneyInvolved,
                InterCountryRelationTypeId,
                DocumentIdProof
            }
        );
        return new InterCountryRelationInserter(command);
    }
}
internal sealed class InterCountryRelationInserter : DatabaseInserter<InterCountryRelation>
{
    internal InterCountryRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(InterCountryRelation interCountryRelation)
    {
        if (interCountryRelation.Id is null)
            throw new NullReferenceException();
        Set(InterCountryRelationInserterFactory.Id, interCountryRelation.Id.Value);
        Set(InterCountryRelationInserterFactory.CountryIdFrom, interCountryRelation.CountryIdFrom);
        Set(InterCountryRelationInserterFactory.CountryIdTo, interCountryRelation.CountryIdTo);
        Set(InterCountryRelationInserterFactory.DateRange, interCountryRelation.DateTimeRange);
        Set(InterCountryRelationInserterFactory.InterCountryRelationTypeId, interCountryRelation.InterCountryRelationTypeId);
        Set(InterCountryRelationInserterFactory.NumberOfChildrenInvolved, interCountryRelation.NumberOfChildrenInvolved);
        Set(InterCountryRelationInserterFactory.MoneyInvolved, interCountryRelation.MoneyInvolved);
        Set(InterCountryRelationInserterFactory.DocumentIdProof, interCountryRelation.DocumentIdProof);
        await _command.ExecuteNonQueryAsync();
    }
}
