namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = NodeTypeInserterFactory;
using Request = NodeType;
using Inserter = NodeTypeInserter;

internal sealed class NodeTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    internal static NonNullableBooleanDatabaseParameter AuthorSpecific = new() { Name = "author_specific" };

    public override string TableName => "node_type";

}
internal sealed class NodeTypeInserter : IdentifiableDatabaseInserter<Request>
{
    public NodeTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Name, request.Name),
            ParameterValue.Create(Factory.Description, request.Description),
            ParameterValue.Create(Factory.AuthorSpecific, request.AuthorSpecific),
        };
    }
}
