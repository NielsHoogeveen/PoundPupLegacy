namespace PoundPupLegacy.EditModel.Readers;

using Request = TopicExistsRequest;

public sealed record TopicExistsRequest : IRequest
{
    public required string Name { get; set; }
}

internal sealed class TopicExistsFactory : DoesRecordExistDatabaseReaderFactory<Request>
{
    private static readonly NonNullableStringDatabaseParameter NameParameter = new() { Name = "name" };

    public override string Sql => SQL;
    private const string SQL = $"""
        select
            t.id
            from term t
            join system_group sg on sg.id = 0
            where t.vocabulary_id = sg.vocabulary_id_tagging and t.name = @name
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] { 
            ParameterValue.Create(NameParameter, request.Name)
        };
    }

}
