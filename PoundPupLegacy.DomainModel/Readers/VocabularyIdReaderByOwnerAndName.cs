namespace PoundPupLegacy.DomainModel.Readers;

using Request = VocabularyIdReaderByOwnerAndNameRequest;

public sealed record VocabularyIdReaderByOwnerAndNameRequest : IRequest
{
    public required int OwnerId { get; init; }
    public required string Name { get; init; }
}

internal sealed class VocabularyIdReaderByOwnerAndNameFactory : IntDatabaseReaderFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter OwnerId = new() { Name = "owner_id" };
    private static readonly NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    private static readonly IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT id FROM vocabulary WHERE owner_id = @owner_id AND name = @name
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(OwnerId, request.OwnerId),
            ParameterValue.Create(Name, request.Name)
        };
    }
    protected override IntValueReader IntValueReader => IdReader;

    protected override string GetErrorMessage(Request request)
    {
        return $"vocabulary {request.Name} cannot be found for owner {request.OwnerId}";
    }
}
