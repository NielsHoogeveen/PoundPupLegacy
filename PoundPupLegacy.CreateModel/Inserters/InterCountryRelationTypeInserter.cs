namespace PoundPupLegacy.CreateModel.Inserters;

using Request = InterCountryRelationType.ToCreate;

internal sealed class InterCountryRelationTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableBooleanDatabaseParameter IsSymmetric = new() { Name = "is_symmetric" };
    public override string TableName => "inter_country_relation_type";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(IsSymmetric, request.EndoRelationTypeDetails.IsSymmetric),
        };
    }
}
