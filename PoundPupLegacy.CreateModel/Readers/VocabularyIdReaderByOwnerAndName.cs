namespace PoundPupLegacy.CreateModel.Readers;

using Factory  = VocabularyIdReaderByOwnerAndNameFactory;
using Reader   = VocabularyIdReaderByOwnerAndName;

public sealed class VocabularyIdReaderByOwnerAndNameFactory : DatabaseReaderFactory<Reader>
{
    internal static NonNullableIntegerDatabaseParameter OwnerId = new() { Name = "owner_id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    internal static IntValueReader IdReader = new() { Name = "id"};

    public override string Sql => SQL;

    const string SQL = """
        SELECT id FROM vocabulary WHERE owner_id = @owner_id AND name = @name
        """;
}
public sealed class VocabularyIdReaderByOwnerAndName : IntDatabaseReader<Reader.Request>
{
    public record Request
    {
        public required int OwnerId { get; init; }
        public required string Name { get; init; }

    }

    internal VocabularyIdReaderByOwnerAndName(NpgsqlCommand command) : base(command) { }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.OwnerId, request.OwnerId),
            ParameterValue.Create(Factory.Name, request.Name)
        };
    }
    protected override IntValueReader IntValueReader => Factory.IdReader;

    protected override string GetErrorMessage(Request request)
    {
        return $"vocabulary {request.Name} cannot be found for owner {request.OwnerId}";
    }
}
