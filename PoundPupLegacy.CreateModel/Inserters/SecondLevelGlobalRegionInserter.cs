namespace PoundPupLegacy.CreateModel.Inserters;
public class SecondLevelGlobalRegionInserterFactory : BasicDatabaseInserterFactory<SecondLevelGlobalRegion, SecondLevelGlobalRegionInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter FirstLevelGlobalRegionId = new() { Name = "first_level_global_region_id" };

    public override string TableName => "second_level_global_region";
}
public class SecondLevelGlobalRegionInserter : BasicDatabaseInserter<SecondLevelGlobalRegion>
{

    public SecondLevelGlobalRegionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(SecondLevelGlobalRegion item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(SecondLevelGlobalRegionInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(SecondLevelGlobalRegionInserterFactory.FirstLevelGlobalRegionId, item.FirstLevelGlobalRegionId),
        };
    }
}
