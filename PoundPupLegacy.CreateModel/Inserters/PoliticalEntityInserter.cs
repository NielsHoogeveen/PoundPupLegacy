namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PoliticalEntityInserterFactory : BasicDatabaseInserterFactory<PoliticalEntity, PoliticalEntityInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableIntegerDatabaseParameter FileIdFlag = new() { Name = "file_id_flag" };

    public override string TableName => "political_entity";
}
internal sealed class PoliticalEntityInserter : BasicDatabaseInserter<PoliticalEntity>
{

    public PoliticalEntityInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(PoliticalEntity item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(PoliticalEntityInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(PoliticalEntityInserterFactory.FileIdFlag, item.FileIdFlag),
        };
    }
}
