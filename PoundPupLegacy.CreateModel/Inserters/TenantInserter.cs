namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = TenantInserterFactory;
using Request = Tenant;
using Inserter = TenantInserter;

internal sealed class TenantInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableStringDatabaseParameter DomainName = new() { Name = "domain_name" };
    internal static NullableIntegerDatabaseParameter VocabularyIdTagging = new() { Name = "vocabulary_id_tagging" };
    internal static NullCheckingIntegerDatabaseParameter AccessRoleIdNotLoggedIn = new() { Name = "access_role_id_not_logged_in" };

    public override string TableName => "tenant";
}
internal sealed class TenantInserter : IdentifiableDatabaseInserter<Request>
{
    public TenantInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.DomainName, request.DomainName),
            ParameterValue.Create(Factory.VocabularyIdTagging, request.VocabularyIdTagging),
            ParameterValue.Create(Factory.AccessRoleIdNotLoggedIn, request.AccessRoleNotLoggedIn?.Id),
        };
    }
}
