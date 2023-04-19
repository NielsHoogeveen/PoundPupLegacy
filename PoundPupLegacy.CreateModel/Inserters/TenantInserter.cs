namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Tenant;

internal sealed class TenantInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableStringDatabaseParameter DomainName = new() { Name = "domain_name" };
    private static readonly NullableIntegerDatabaseParameter VocabularyIdTagging = new() { Name = "vocabulary_id_tagging" };
    private static readonly NullCheckingIntegerDatabaseParameter AccessRoleIdNotLoggedIn = new() { Name = "access_role_id_not_logged_in" };

    public override string TableName => "tenant";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(DomainName, request.DomainName),
            ParameterValue.Create(VocabularyIdTagging, request.VocabularyIdTagging),
            ParameterValue.Create(AccessRoleIdNotLoggedIn, request.AccessRoleNotLoggedIn?.Id),
        };
    }
}
