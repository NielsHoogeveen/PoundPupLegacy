using PoundPupLegacy.Common;
using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Updaters;

internal sealed class BlogPostChangerFactory(
    IDatabaseUpdaterFactory<BlogPost.ToUpdate> databaseUpdaterFactory,
    NodeDetailsChangerFactory nodeDetailsChangerFactory) : IEntityChangerFactory<BlogPost.ToUpdate>
{
    public async Task<IEntityChanger<BlogPost.ToUpdate>> CreateAsync(IDbConnection connection)
    {
        return new NodeChanger<BlogPost.ToUpdate>(
            await databaseUpdaterFactory.CreateAsync(connection),
            await nodeDetailsChangerFactory.CreateAsync(connection)
        );
    }
}

internal sealed class BlogPostUpdaterFactory : SimpleTextNodeUpdaterFactory<BlogPost.ToUpdate> { }