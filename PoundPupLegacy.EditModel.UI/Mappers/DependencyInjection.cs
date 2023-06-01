using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal static class DependencyInjection
{
    public static void AddMappers(this IServiceCollection services)
    {
        services.AddTransient<IMapper<EditModel.BlogPost.ToUpdate, CreateModel.BlogPost.ToUpdate>, BlogPostToUpdateMapper>();
        services.AddTransient<IMapper<EditModel.BlogPost.ToCreate, CreateModel.BlogPost.ToCreate>, BlogPostToCreateMapper>();
        services.AddTransient<IMapper<EditModel.NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate>, NodeDetailsForUpdateMapper>();
        services.AddTransient<IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate>, NodeDetailsForCreateMapper>();
        services.AddTransient<IEnumerableMapper<Tags.ToUpdate, ResolvedNodeTermToAdd>, NodeTermToAddForUpdateMapper>();
        services.AddTransient<IEnumerableMapper<Tags.ToUpdate, NodeTermToRemove>, NodeTermToRemoveMapper>();
        services.AddTransient<IEnumerableMapper<EditModel.TenantNode.ToCreateForExistingNode, CreateModel.TenantNode.ToCreate.ForExistingNode>, TenantNodeToCreateForExistingNodeMapper>();
        services.AddTransient<IEnumerableMapper<EditModel.TenantNode.ToUpdate, CreateModel.Deleters.TenantNodeToDelete>, TenantNodeToRemoveMapper>();
        services.AddTransient<IEnumerableMapper<EditModel.TenantNode.ToUpdate, CreateModel.TenantNode.ToUpdate>, TenantNodeToUpdateMapper>();
        services.AddTransient<IEnumerableMapper<Tags.ToCreate, int>, NodeTermToAddForCreateMapper>();
        services.AddTransient<IEnumerableMapper<EditModel.TenantNode.ToCreateForNewNode, CreateModel.TenantNode.ToCreate.ForNewNode>, TenantNodesToCreateForNewNodeMapper>();

    }
}
