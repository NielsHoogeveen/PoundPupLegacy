namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class BasicSecondLevelSubdivisionInserterFactory : DatabaseInserterFactory<BasicSecondLevelSubdivision, BasicSecondLevelSubdivisionInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter IntermediateLevelSubdivisionId = new() { Name = "intermediate_level_subdivision_id" };
    public override string TableName => "basic_second_level_subdivision";
}
internal sealed class BasicSecondLevelSubdivisionInserter : DatabaseInserter<BasicSecondLevelSubdivision>
{
    public BasicSecondLevelSubdivisionInserter(NpgsqlCommand command) : base(command)
    {
    }
    protected override IEnumerable<ParameterValue> GetParameterValues(BasicSecondLevelSubdivision item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(BasicSecondLevelSubdivisionInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(BasicSecondLevelSubdivisionInserterFactory.IntermediateLevelSubdivisionId, item.IntermediateLevelSubdivisionId)
        };
    }
}
