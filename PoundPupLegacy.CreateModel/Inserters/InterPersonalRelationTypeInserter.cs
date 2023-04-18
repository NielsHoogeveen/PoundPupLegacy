namespace PoundPupLegacy.CreateModel.Inserters;

using Request = InterPersonalRelationType;

internal sealed class InterPersonalRelationTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableBooleanDatabaseParameter IsSymmetric = new() { Name = "is_symmetric" };

    public override string TableName => "inter_personal_relation_type";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(IsSymmetric, request.IsSymmetric),
        };
    }
}
