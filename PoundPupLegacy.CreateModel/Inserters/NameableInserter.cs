namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class NameableInserterFactory : DatabaseInserterFactory<Nameable, NameableInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableStringDatabaseParameter Description = new() { Name = "description" };
    internal static NullableIntegerDatabaseParameter FileIdTileImage = new() { Name = "file_id_tile_image" };

    public override string TableName => "nameable";

}
internal sealed class NameableInserter : DatabaseInserter<Nameable>
{
    public NameableInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Nameable item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(NameableInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(NameableInserterFactory.Description, item.Description),
            ParameterValue.Create(NameableInserterFactory.FileIdTileImage, item.FileIdTileImage),
        };
    }
}
