namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = NameableInserterFactory;
using Request = Nameable;
using Inserter = NameableInserter;

internal sealed class NameableInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NullableStringDatabaseParameter Description = new() { Name = "description" };
    internal static NullableIntegerDatabaseParameter FileIdTileImage = new() { Name = "file_id_tile_image" };

    public override string TableName => "nameable";

}
internal sealed class NameableInserter : IdentifiableDatabaseInserter<Request>
{
    public NameableInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Description, request.Description),
            ParameterValue.Create(Factory.FileIdTileImage, request.FileIdTileImage),
        };
    }
}
