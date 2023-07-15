using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

using Request = CollectiveUser;

internal sealed class CollectiveUserInserterFactory : BasicDatabaseInserterFactory<Request>
{
    internal static NullCheckingIntegerDatabaseParameter CollectiveId = new() { Name = "collective_id" };
    internal static NullCheckingIntegerDatabaseParameter UserId = new() { Name = "user_id" };

    public override string TableName => "collective_user";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CollectiveId, request.CollectiveId),
            ParameterValue.Create(UserId, request.UserId)
        };
    }
}
