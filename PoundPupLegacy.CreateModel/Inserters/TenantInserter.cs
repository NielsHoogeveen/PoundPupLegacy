namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TenantInserterFactory : BasicDatabaseInserterFactory<Tenant, TenantInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter DomainName = new() { Name = "domain_name" };
    internal static NullableIntegerDatabaseParameter VocabularyIdTagging = new() { Name = "vocabulary_id_tagging" };
    internal static NonNullableIntegerDatabaseParameter AccessRoleIdNotLoggedIn = new() { Name = "access_role_id_not_logged_in" };

    public override string TableName => "tenant";
}
internal sealed class TenantInserter : BasicDatabaseInserter<Tenant>
{
    public TenantInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(Tenant item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        if (item.AccessRoleNotLoggedIn.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(TenantInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(TenantInserterFactory.DomainName, item.DomainName),
            ParameterValue.Create(TenantInserterFactory.VocabularyIdTagging, item.VocabularyIdTagging),
            ParameterValue.Create(TenantInserterFactory.AccessRoleIdNotLoggedIn, item.AccessRoleNotLoggedIn.Id.Value),
        };
    }
}
