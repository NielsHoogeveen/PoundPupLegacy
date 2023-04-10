namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PublishingUserGroupInserterFactory : DatabaseInserterFactory<PublishingUserGroup, PublishingUserGroupInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter PublicationStatusIdDefault = new() { Name = "publication_status_id_default" };

    public override string TableName => "publishing_user_group";
}
internal sealed class PublishingUserGroupInserter : DatabaseInserter<PublishingUserGroup>
{

    public PublishingUserGroupInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(PublishingUserGroup item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(PublishingUserGroupInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(PublishingUserGroupInserterFactory.PublicationStatusIdDefault, item.PublicationStatusIdDefault),
        };
    }
}
