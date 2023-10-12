namespace PoundPupLegacy.DomainModel.Updaters;

using Request = NodeDetails.ForUpdate;

internal sealed class NodeDetailsUpdaterFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    private static readonly NonNullableDateTimeDatabaseParameter LastChangedDateTIme = new() { Name = "changed_date_time" };

    public override string Sql => $"""
        update node 
        set 
        changed_date_time = @changed_date_time
        where id = @id
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(Id, request.Id),
            ParameterValue.Create(LastChangedDateTIme, request.ChangedDateTime),
        };
    }
}

