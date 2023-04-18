namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Tenant;

internal sealed class TenantInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableStringDatabaseParameter DomainName = new() { Name = "domain_name" };
    internal static NullableIntegerDatabaseParameter VocabularyIdTagging = new() { Name = "vocabulary_id_tagging" };
    internal static NullCheckingIntegerDatabaseParameter AccessRoleIdNotLoggedIn = new() { Name = "access_role_id_not_logged_in" };

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
