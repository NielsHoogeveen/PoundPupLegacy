namespace PoundPupLegacy.CreateModel.Inserters;

using Request = InterOrganizationalRelationType.InterOrganizationalRelationTypeToCreate;

internal sealed class InterOrganizationalRelationTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableBooleanDatabaseParameter IsSymmetric = new() { Name = "is_symmetric" };
    public override string TableName => "inter_organizational_relation_type";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(IsSymmetric, request.EndoRelationTypeDetails.IsSymmetric),
        };
    }
}
