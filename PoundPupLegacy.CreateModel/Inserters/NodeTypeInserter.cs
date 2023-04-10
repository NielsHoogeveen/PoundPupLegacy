namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class NodeTypeInserterFactory : DatabaseInserterFactory<NodeType>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    internal static NonNullableBooleanDatabaseParameter AuthorSpecific = new() { Name = "author_specific" };

    public override async Task<IDatabaseInserter<NodeType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "node_type",
            new DatabaseParameter[] {
                Id,
                Name,
                Description,
                AuthorSpecific
            }
        );
        return new NodeTypeInserter(command);
    }
}
internal sealed class NodeTypeInserter : DatabaseInserter<NodeType>
{
    internal NodeTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(NodeType nodeType)
    {
        if (nodeType.Id is null)
            throw new NullReferenceException();
        Set(NodeTypeInserterFactory.Id, nodeType.Id.Value);
        Set(NodeTypeInserterFactory.Name, nodeType.Name);
        Set(NodeTypeInserterFactory.Description, nodeType.Description);
        Set(NodeTypeInserterFactory.AuthorSpecific, nodeType.AuthorSpecific);
        await _command.ExecuteNonQueryAsync();
    }
}
