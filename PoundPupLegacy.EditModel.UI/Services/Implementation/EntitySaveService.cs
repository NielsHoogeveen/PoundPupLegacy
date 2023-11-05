using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using PoundPupLegacy.DomainModel;
using PoundPupLegacy.DomainModel.Updaters;
using PoundPupLegacy.EditModel.Mappers;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

public static class EntitySaveServiceExensions
{
    public static void AddEntitySaveService<
    TEditModel,
    TUpdateModel,
    TCreateModel,
    TDomainModel,
    TDomainModelToUpdate,
    TDomainModelToCreate
    >(this IServiceCollection services)
    where TEditModel : class, Node
    where TUpdateModel : class, TEditModel, ExistingNode
    where TCreateModel : class, TEditModel, ResolvedNewNode
    where TDomainModel : class, DomainModel.Node
    where TDomainModelToUpdate : class, NodeToUpdate
    where TDomainModelToCreate : class, NodeToCreate
    {
        services.AddTransient<IEntitySaveService<TUpdateModel, TCreateModel>> (serviceProvider => 
        {
            NpgsqlDataSource dataSource = serviceProvider.GetRequiredService<NpgsqlDataSource>();
            ILogger logger = serviceProvider.GetRequiredService<ILogger<TEditModel>>();
            IMapper<TUpdateModel, TDomainModelToUpdate> updateMapper = serviceProvider.GetRequiredService<IMapper<TUpdateModel, TDomainModelToUpdate>>();
            IMapper<TCreateModel, TDomainModelToCreate> createMapper = serviceProvider.GetRequiredService<IMapper<TCreateModel, TDomainModelToCreate>>();
            IEntityChangerFactory<TDomainModelToUpdate> entityChanger = serviceProvider.GetRequiredService<IEntityChangerFactory<TDomainModelToUpdate>>();
            IEntityCreatorFactory<TDomainModelToCreate> entityCreator = serviceProvider.GetRequiredService<IEntityCreatorFactory<TDomainModelToCreate>>();
            return new EntitySaveService<
                TEditModel,
                TUpdateModel,
                TCreateModel,
                TDomainModel,
                TDomainModelToUpdate,
                TDomainModelToCreate
                >(
                dataSource,
                    logger,
                    updateMapper,
                    createMapper,
                    entityChanger,
                    entityCreator
                );
        });
    }

    
}


internal class EntitySaveService<
    TEditModel,
    TUpdateModel,
    TCreateModel, 
    TDomainModel,
    TDomainModelToUpdate,
    TDomainModelToCreate
    >
    (
        NpgsqlDataSource dataSource,
        ILogger logger,
        IMapper<TUpdateModel, TDomainModelToUpdate> updateMapper,
        IMapper<TCreateModel, TDomainModelToCreate> createMapper,
        IEntityChangerFactory<TDomainModelToUpdate> entityChanger,
        IEntityCreatorFactory<TDomainModelToCreate> entityCreator
    ) : DatabaseService(dataSource, logger), IEntitySaveService<TUpdateModel, TCreateModel>
    where TEditModel : class, Node
    where TCreateModel: class, TEditModel, ResolvedNewNode
    where TUpdateModel: class, TEditModel, ExistingNode
    where TDomainModel: class, DomainModel.Node
    where TDomainModelToCreate: class, NodeToCreate
    where TDomainModelToUpdate : class, NodeToUpdate
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
            return node.NodeIdentification.NodeId;
        });
    }
}
