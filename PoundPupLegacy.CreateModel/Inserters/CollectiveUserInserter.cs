namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CollectiveUserInserterFactory : DatabaseInserterFactory<CollectiveUser, CollectiveUserInserter>
{
    internal static NonNullableIntegerDatabaseParameter CollectiveId = new() { Name = "collective_id" };
    internal static NonNullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };

    public override string TableName => "collective_user";

}
internal sealed class CollectiveUserInserter : DatabaseInserter<CollectiveUser>
{
    public CollectiveUserInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(CollectiveUser item)
    {
        if (item.CollectiveId is null || item.UserId is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(CollectiveUserInserterFactory.CollectiveId, item.CollectiveId.Value),
            ParameterValue.Create(CollectiveUserInserterFactory.UserId, item.UserId.Value)
        };
    }
}
