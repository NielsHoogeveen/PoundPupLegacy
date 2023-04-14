namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class InterCountryRelationInserterFactory : DatabaseInserterFactory<InterCountryRelation, InterCountryRelationInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter CountryIdFrom = new() { Name = "country_id_from" };
    internal static NonNullableIntegerDatabaseParameter CountryIdTo = new() { Name = "country_id_to" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NullableIntegerDatabaseParameter NumberOfChildrenInvolved = new() { Name = "number_of_children_involved" };
    internal static NullableDecimalDatabaseParameter MoneyInvolved = new() { Name = "money_involved" };
    internal static NonNullableIntegerDatabaseParameter InterCountryRelationTypeId = new() { Name = "inter_country_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };

    public override string TableName => "inter_country_relation";

}
internal sealed class InterCountryRelationInserter : DatabaseInserter<InterCountryRelation>
{
    public InterCountryRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(InterCountryRelation item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(InterCountryRelationInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(InterCountryRelationInserterFactory.CountryIdFrom, item.CountryIdFrom),
            ParameterValue.Create(InterCountryRelationInserterFactory.CountryIdTo, item.CountryIdTo),
            ParameterValue.Create(InterCountryRelationInserterFactory.DateRange, item.DateTimeRange),
            ParameterValue.Create(InterCountryRelationInserterFactory.InterCountryRelationTypeId, item.InterCountryRelationTypeId),
            ParameterValue.Create(InterCountryRelationInserterFactory.NumberOfChildrenInvolved, item.NumberOfChildrenInvolved),
            ParameterValue.Create(InterCountryRelationInserterFactory.MoneyInvolved, item.MoneyInvolved),
            ParameterValue.Create(InterCountryRelationInserterFactory.DocumentIdProof, item.DocumentIdProof),
        };
    }
}
