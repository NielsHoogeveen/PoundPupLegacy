namespace PoundPupLegacy.EditModel.Updaters;

using Request = DeportationCaseUpdaterRequest;

public record DeportationCaseUpdaterRequest : IRequest
{
    public required int NodeId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required FuzzyDate? Date { get; init; }
    public required int? SubdivisionIdFrom { get; init; }
    public required int? CountryIdTo { get; init; }

}
internal sealed class DeportationCaseUpdaterFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    private static readonly NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    private static readonly NullableFuzzyDateDatabaseParameter FuzzyDate = new() { Name = "fuzzy_date" };
    private static readonly NullableIntegerDatabaseParameter SubdivisionIdFrom = new() { Name = "subdivision_id_from" };
    private static readonly NullableIntegerDatabaseParameter CountryIdTo = new() { Name = "country_id_to" };


    public override string Sql => $"""
        update node 
        set 
            title=@title
        where id = @node_id;
        update nameable 
        set 
            description=@description
        where id = @node_id;
        update "case"
        set 
            fuzzy_date=@fuzzy_date
        where id = @node_id;
        update deportation_case 
        set 
            subdivision_id_from=@subdivision_id_from,
            country_id_to =@country_id_to
        where id = @node_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(NodeId, request.NodeId),
            ParameterValue.Create(Title, request.Title),
            ParameterValue.Create(Description, request.Description),
            ParameterValue.Create(FuzzyDate, request.Date),
            ParameterValue.Create(SubdivisionIdFrom, request.SubdivisionIdFrom),
            ParameterValue.Create(CountryIdTo, request.CountryIdTo),
        };
    }
}

