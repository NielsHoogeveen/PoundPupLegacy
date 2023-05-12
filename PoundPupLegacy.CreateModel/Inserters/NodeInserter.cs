namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Node;

public class NodeInserterFactory : AutoGenerateIdDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter PublisherId = new() { Name = "publisher_id" };
    private static readonly NonNullableDateTimeDatabaseParameter CreatedDateTime = new() { Name = "created_date_time" };
    private static readonly NonNullableDateTimeDatabaseParameter ChangedDateTime = new() { Name = "changed_date_time" };
    private static readonly TrimmingNonNullableStringDatabaseParameter Title = new() { Name = "title" };
    private static readonly NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };
    private static readonly NonNullableIntegerDatabaseParameter OwnerId = new() { Name = "owner_id" };
    private static readonly NonNullableIntegerDatabaseParameter AuthorizationStatusId = new() { Name = "authorization_status_id" };

    public override string TableName => "node";
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(PublisherId,request.PublisherId),
            ParameterValue.Create(CreatedDateTime, request.CreatedDateTime),
            ParameterValue.Create(ChangedDateTime, request.ChangedDateTime),
            ParameterValue.Create(Title, request.Title),
            ParameterValue.Create(NodeTypeId, request.NodeTypeId),
            ParameterValue.Create(OwnerId, request.OwnerId),
            ParameterValue.Create(AuthorizationStatusId, request.AuthoringStatusId),
        };
    }
}
