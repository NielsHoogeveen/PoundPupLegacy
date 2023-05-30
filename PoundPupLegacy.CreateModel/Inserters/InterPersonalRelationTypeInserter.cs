namespace PoundPupLegacy.CreateModel.Inserters;

using Request = InterPersonalRelationType.ToCreate;

internal sealed class InterPersonalRelationTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static NonNullableBooleanDatabaseParameter IsSymmetric = new() { Name = "is_symmetric" };

    public override string TableName => "inter_personal_relation_type";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(IsSymmetric, request.EndoRelationTypeDetails.IsSymmetric),
        };
    }
}
