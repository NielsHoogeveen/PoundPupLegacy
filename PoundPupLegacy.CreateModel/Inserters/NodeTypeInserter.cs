namespace PoundPupLegacy.CreateModel.Inserters;

using Request = NodeType;

internal sealed class NodeTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    internal static NonNullableBooleanDatabaseParameter AuthorSpecific = new() { Name = "author_specific" };

    public override string TableName => "node_type";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Name, request.Name),
            ParameterValue.Create(Description, request.Description),
            ParameterValue.Create(AuthorSpecific, request.AuthorSpecific),
        };
    }
}
