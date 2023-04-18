namespace PoundPupLegacy.CreateModel.Inserters;

using Request = PublishingUserGroup;

internal sealed class PublishingUserGroupInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter PublicationStatusIdDefault = new() { Name = "publication_status_id_default" };

    public override string TableName => "publishing_user_group";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(PublicationStatusIdDefault, request.PublicationStatusIdDefault),
        };
    }
}
