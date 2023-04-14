namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = InterCountryRelationInserterFactory;
using Request = InterCountryRelation;
using Inserter = InterCountryRelationInserter;

internal sealed class InterCountryRelationInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter CountryIdFrom = new() { Name = "country_id_from" };
    internal static NonNullableIntegerDatabaseParameter CountryIdTo = new() { Name = "country_id_to" };
    internal static NullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };
    internal static NullableIntegerDatabaseParameter NumberOfChildrenInvolved = new() { Name = "number_of_children_involved" };
    internal static NullableDecimalDatabaseParameter MoneyInvolved = new() { Name = "money_involved" };
    internal static NonNullableIntegerDatabaseParameter InterCountryRelationTypeId = new() { Name = "inter_country_relation_type_id" };
    internal static NullableIntegerDatabaseParameter DocumentIdProof = new() { Name = "document_id_proof" };

    public override string TableName => "inter_country_relation";

}
internal sealed class InterCountryRelationInserter : IdentifiableDatabaseInserter<Request>
{
    public InterCountryRelationInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.CountryIdFrom, request.CountryIdFrom),
            ParameterValue.Create(Factory.CountryIdTo, request.CountryIdTo),
            ParameterValue.Create(Factory.DateRange, request.DateTimeRange),
            ParameterValue.Create(Factory.InterCountryRelationTypeId, request.InterCountryRelationTypeId),
            ParameterValue.Create(Factory.NumberOfChildrenInvolved, request.NumberOfChildrenInvolved),
            ParameterValue.Create(Factory.MoneyInvolved, request.MoneyInvolved),
            ParameterValue.Create(Factory.DocumentIdProof, request.DocumentIdProof),
        };
    }
}
