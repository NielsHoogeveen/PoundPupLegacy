namespace PoundPupLegacy.CreateModel.Inserters;

using Request = NewInterCountryRelation;

internal sealed class InterCountryRelationInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter CountryIdFrom = new() { Name = "country_id_from" };
    private static readonly NonNullableIntegerDatabaseParameter CountryIdTo = new() { Name = "country_id_to" };
    private static readonly NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    private static readonly NullableIntegerDatabaseParameter NumberOfChildrenInvolved = new() { Name = "number_of_children_involved" };
    private static readonly NullableDecimalDatabaseParameter MoneyInvolved = new() { Name = "money_involved" };
    private static readonly NonNullableIntegerDatabaseParameter InterCountryRelationTypeId = new() { Name = "inter_country_relation_type_id" };
    private static readonly NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };

    public override string TableName => "inter_country_relation";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CountryIdFrom, request.CountryIdFrom),
            ParameterValue.Create(CountryIdTo, request.CountryIdTo),
            ParameterValue.Create(DateRange, request.DateTimeRange),
            ParameterValue.Create(InterCountryRelationTypeId, request.InterCountryRelationTypeId),
            ParameterValue.Create(NumberOfChildrenInvolved, request.NumberOfChildrenInvolved),
            ParameterValue.Create(MoneyInvolved, request.MoneyInvolved),
            ParameterValue.Create(DocumentIdProof, request.DocumentIdProof),
        };
    }
}
