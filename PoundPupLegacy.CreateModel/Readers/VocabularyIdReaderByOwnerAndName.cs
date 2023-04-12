namespace PoundPupLegacy.CreateModel.Readers;
public sealed class VocabularyIdReaderByOwnerAndNameFactory : DatabaseReaderFactory<VocabularyIdReaderByOwnerAndName>
{
    internal static NonNullableIntegerDatabaseParameter OwnerId = new() { Name = "owner_id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT id FROM vocabulary WHERE owner_id = @owner_id AND name = @name
        """;
}
public sealed class VocabularyIdReaderByOwnerAndName : SingleItemDatabaseReader<VocabularyIdReaderByOwnerAndName.Request, int>
{
    public record Request
    {
        public required int OwnerId { get; init; }
        public required string Name { get; init; }

    }

    internal VocabularyIdReaderByOwnerAndName(NpgsqlCommand command) : base(command) { }

    public override async Task<int> ReadAsync(Request request)
    {
        _command.Parameters["owner_id"].Value = request.OwnerId;
        _command.Parameters["name"].Value = request.Name;

        var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var id = reader.GetInt32("id");
            await reader.CloseAsync();
            return id;
        }
        await reader.CloseAsync();
        throw new Exception($"vocabulary {request.Name} cannot be found for owner {request.OwnerId}");
    }
}
