namespace PoundPupLegacy.CreateModel.Inserters;

using Request = InterCountryRelation;

internal sealed class InterCountryRelationInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter CountryIdFrom = new() { Name = "country_id_from" };
    internal static NonNullableIntegerDatabaseParameter CountryIdTo = new() { Name = "country_id_to" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NullableIntegerDatabaseParameter NumberOfChildrenInvolved = new() { Name = "number_of_children_involved" };
    internal static NullableDecimalDatabaseParameter MoneyInvolved = new() { Name = "money_involved" };
    internal static NonNullableIntegerDatabaseParameter InterCountryRelationTypeId = new() { Name = "inter_country_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };

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
