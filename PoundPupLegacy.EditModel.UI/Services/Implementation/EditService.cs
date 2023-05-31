using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel.Updaters;
using PoundPupLegacy.EditModel.UI.Mappers;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal interface IEditService2<TCreateModel,TUpdateModel>
    where TCreateModel : class, ResolvedNewNode
    where TUpdateModel : class, ExistingNode
{
    Task<int> SaveAsync(TCreateModel node);
    Task<int> SaveAsync(TUpdateModel node);
}

internal class EditService<
    TEditModel, 
    TCreateModel, 
    TUpdateModel, 
    TDomainModel,
    TDomainModelToCreate,
    TDomainModelToUpdate>
    (
        IDbConnection connection,
        ILogger logger,
        IMapper<TCreateModel, TDomainModelToCreate> createMapper,
        IMapper<TUpdateModel, TDomainModelToUpdate> updateMapper,
        IEntityCreatorFactory<TDomainModelToCreate> entityCreator,
        IEntityChangerFactory<TDomainModelToUpdate> entityChanger
    ) : DatabaseService(connection, logger), IEditService2<TCreateModel, TUpdateModel>
    where TEditModel : class, Node
    where TCreateModel: class, TEditModel, ResolvedNewNode
    where TUpdateModel: class, TEditModel, ExistingNode
    where TDomainModel: class, CreateModel.Node
    where TDomainModelToCreate: class, CreateModel.NodeToCreate
    where TDomainModelToUpdate : class, CreateModel.NodeToUpdate
{

    public async Task<int> SaveAsync(TCreateModel node)
    {

        return await WithTransactedConnection(async (connection) => {
            await using var creator = await entityCreator.CreateAsync(connection);
            var nodeToCreate = createMapper.Map(node);
            await creator.CreateAsync(nodeToCreate);
            return nodeToCreate.Identification.Id!.Value;
        });
    }

    public async Task<int> SaveAsync(TUpdateModel node)
    {
        return await WithTransactedConnection(async (connection) => {
            await using var updater = await entityChanger.CreateAsync(connection);
            var nodeToUpdate = updateMapper.Map(node);
            await updater.UpdateAsync(nodeToUpdate);
            return node.NodeIdentification.UrlId;
        });
    }
}
