namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Node;

public class NodeInserterFactory : AutoGenerateIdDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter PublisherId = new() { Name = "publisher_id" };
    internal static NonNullableDateTimeDatabaseParameter CreatedDateTime = new() { Name = "created_date_time" };
    internal static NonNullableDateTimeDatabaseParameter ChangedDateTime = new() { Name = "changed_date_time" };
    internal static TrimmingNonNullableStringDatabaseParameter Title = new() { Name = "title" };
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };
    internal static NonNullableIntegerDatabaseParameter OwnerId = new() { Name = "owner_id" };

    public override string TableName => "node";
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(PublisherId,request.PublisherId),
            ParameterValue.Create(CreatedDateTime, request.CreatedDateTime),
            ParameterValue.Create(ChangedDateTime, request.ChangedDateTime),
            ParameterValue.Create(Title, request.Title),
            ParameterValue.Create(NodeTypeId, request.NodeTypeId),
            ParameterValue.Create(OwnerId, request.OwnerId)
        };
    }
}
