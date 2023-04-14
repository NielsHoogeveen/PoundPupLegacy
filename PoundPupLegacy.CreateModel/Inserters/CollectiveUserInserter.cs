namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = CollectiveUserInserterFactory;
using Request = CollectiveUser;
using Inserter = CollectiveUserInserter;

internal sealed class CollectiveUserInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NullCheckingIntegerDatabaseParameter CollectiveId = new() { Name = "collective_id" };
    internal static NullCheckingIntegerDatabaseParameter UserId = new() { Name = "user_id" };

    public override string TableName => "collective_user";

}
internal sealed class CollectiveUserInserter : DatabaseInserter<Request>
{
    public CollectiveUserInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.CollectiveId, request.CollectiveId),
            ParameterValue.Create(Factory.UserId, request.UserId)
        };
    }
}
