namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = PublishingUserGroupInserterFactory;
using Request = PublishingUserGroup;
using Inserter = PublishingUserGroupInserter;

internal sealed class PublishingUserGroupInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter PublicationStatusIdDefault = new() { Name = "publication_status_id_default" };

    public override string TableName => "publishing_user_group";
}
internal sealed class PublishingUserGroupInserter : IdentifiableDatabaseInserter<Request>
{

    public PublishingUserGroupInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.PublicationStatusIdDefault, request.PublicationStatusIdDefault),
        };
    }
}
