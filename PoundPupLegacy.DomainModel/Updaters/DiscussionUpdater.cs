using PoundPupLegacy.Common;
using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Updaters;

internal sealed class DiscussionChangerFactory(
    IDatabaseUpdaterFactory<Discussion.ToUpdate> databaseUpdaterFactory,
    NodeDetailsChangerFactory nodeDetailsChangerFactory) : IEntityChangerFactory<Discussion.ToUpdate>
{
    public async Task<IEntityChanger<Discussion.ToUpdate>> CreateAsync(IDbConnection connection)
    {
        return new NodeChanger<Discussion.ToUpdate>(
            await databaseUpdaterFactory.CreateAsync(connection),
            await nodeDetailsChangerFactory.CreateAsync(connection)
        );
    }
}

internal sealed class DiscussionUpdaterFactory : SimpleTextNodeUpdaterFactory<Discussion.ToUpdate> { }