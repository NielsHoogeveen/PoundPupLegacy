namespace PoundPupLegacy.DomainModel.Inserters;

using Request = Tenant;

internal sealed class TenantInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableStringDatabaseParameter DomainName = new() { Name = "domain_name" };
    private static readonly NullableStringDatabaseParameter FrontPageText = new() { Name = "front_page_text" };
    private static readonly NullableStringDatabaseParameter Logo = new() { Name = "logo" };
    private static readonly NullableStringDatabaseParameter SubTitle = new() { Name = "sub_title" };
    private static readonly NullableStringDatabaseParameter FooterText = new() { Name = "footer_text" };
    private static readonly NullableStringDatabaseParameter CssFile = new() { Name = "css_file" };
    private static readonly NullCheckingIntegerDatabaseParameter AccessRoleIdNotLoggedIn = new() { Name = "access_role_id_not_logged_in" };

    public override string TableName => "tenant";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(DomainName, request.DomainName),
            ParameterValue.Create(FrontPageText, request.FrontPageText),
            ParameterValue.Create(Logo, request.Logo),
            ParameterValue.Create(SubTitle, request.SubTitle),
            ParameterValue.Create(FooterText, request.FooterText),
            ParameterValue.Create(CssFile, request.CssFile),
            ParameterValue.Create(AccessRoleIdNotLoggedIn, request.AccessRoleNotLoggedIn.Identification?.Id),
        };
    }
}
