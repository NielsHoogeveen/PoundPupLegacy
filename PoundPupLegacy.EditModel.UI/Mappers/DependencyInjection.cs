using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal static class DependencyInjection
{
    public static void AddMappers(this IServiceCollection services)
    {
        services.AddTransient<IMapper<EditModel.AbuseCase.ToUpdate, CreateModel.AbuseCase.ToUpdate>, AbuseCaseToUpdateMapper>();
        services.AddTransient<IMapper<EditModel.AbuseCase.ToCreate, CreateModel.AbuseCase.ToCreate>, AbuseCaseToCreateMapper>();

        services.AddTransient<IMapper<EditModel.BlogPost.ToUpdate, CreateModel.BlogPost.ToUpdate>, BlogPostToUpdateMapper>();
        services.AddTransient<IMapper<EditModel.BlogPost.ToCreate, CreateModel.BlogPost.ToCreate>, BlogPostToCreateMapper>();
        services.AddTransient<IMapper<EditModel.ChildTraffickingCase.ToUpdate, CreateModel.ChildTraffickingCase.ToUpdate>, ChildTraffickingCaseToUpdateMapper>();
        services.AddTransient<IMapper<EditModel.ChildTraffickingCase.ToCreate, CreateModel.ChildTraffickingCase.ToCreate>, ChildTraffickingCaseToCreateMapper>();

        services.AddTransient<IMapper<EditModel.CoercedAdoptionCase.ToUpdate, CreateModel.CoercedAdoptionCase.ToUpdate>, CoercedAdoptionCaseToUpdateMapper>();
        services.AddTransient<IMapper<EditModel.CoercedAdoptionCase.ToCreate, CreateModel.CoercedAdoptionCase.ToCreate>, CoercedAdoptionCaseToCreateMapper>();

        services.AddTransient<IMapper<EditModel.DeportationCase.ToUpdate, CreateModel.DeportationCase.ToUpdate>, DeportationCaseToUpdateMapper>();
        services.AddTransient<IMapper<EditModel.DeportationCase.ToCreate, CreateModel.DeportationCase.ToCreate>, DeportationCaseToCreateMapper>();

        services.AddTransient<IMapper<EditModel.DisruptedPlacementCase.ToUpdate, CreateModel.DisruptedPlacementCase.ToUpdate>, DisruptedPlacementCaseToUpdateMapper>();
        services.AddTransient<IMapper<EditModel.DisruptedPlacementCase.ToCreate, CreateModel.DisruptedPlacementCase.ToCreate>, DisruptedPlacementCaseToCreateMapper>();

        services.AddTransient<IMapper<EditModel.Document.ToUpdate, CreateModel.Document.ToUpdate>, DocumentToUpdateMapper>();
        services.AddTransient<IMapper<EditModel.Document.ToCreate, CreateModel.Document.ToCreate>, DocumentToCreateMapper>();

        services.AddTransient<IMapper<EditModel.FathersRightsViolationCase.ToUpdate, CreateModel.FathersRightsViolationCase.ToUpdate>, FathersRightsViolationCaseToUpdateMapper>();
        services.AddTransient<IMapper<EditModel.FathersRightsViolationCase.ToCreate, CreateModel.FathersRightsViolationCase.ToCreate>, FathersRightsViolationCaseToCreateMapper>();

        services.AddTransient<IMapper<EditModel.NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate>, NodeDetailsForUpdateMapper>();
        services.AddTransient<IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate>, NodeDetailsForCreateMapper>();

        services.AddTransient<IMapper<EditModel.Organization.ToUpdate, CreateModel.OrganizationToUpdate>, OrganizationToUpdateMapper>();
        services.AddTransient<IMapper<EditModel.Organization.ToCreate, CreateModel.OrganizationToCreate>, OrganizationToCreateMapper>();

        services.AddTransient<IMapper<EditModel.Person.ToUpdate, CreateModel.Person.ToUpdate>, PersonToUpdateMapper>();
        services.AddTransient<IMapper<EditModel.Person.ToCreate, CreateModel.Person.ToCreate>, PersonToCreateMapper>();

        services.AddTransient<IEnumerableMapper<Tags.ToUpdate, ResolvedNodeTermToAdd>, NodeTermToAddForUpdateMapper>();
        services.AddTransient<IEnumerableMapper<Tags.ToUpdate, NodeTermToRemove>, NodeTermToRemoveMapper>();
        services.AddTransient<IEnumerableMapper<Tags.ToCreate, int>, NodeTermToAddForCreateMapper>();

        services.AddTransient<IEnumerableMapper<EditModel.TenantNode.ToCreateForExistingNode, CreateModel.TenantNode.ToCreate.ForExistingNode>, TenantNodeToCreateForExistingNodeMapper>();
        services.AddTransient<IEnumerableMapper<EditModel.TenantNode.ToUpdate, CreateModel.Deleters.TenantNodeToDelete>, TenantNodeToRemoveMapper>();
        services.AddTransient<IEnumerableMapper<EditModel.TenantNode.ToUpdate, CreateModel.TenantNode.ToUpdate>, TenantNodeToUpdateMapper>();
        services.AddTransient<IEnumerableMapper<EditModel.TenantNode.ToCreateForNewNode, CreateModel.TenantNode.ToCreate.ForNewNode>, TenantNodesToCreateForNewNodeMapper>();

        services.AddTransient<IMapper<EditModel.WrongfulMedicationCase.ToUpdate, CreateModel.WrongfulMedicationCase.ToUpdate>, WrongfulMedicationCaseToUpdateMapper>();
        services.AddTransient<IMapper<EditModel.WrongfulMedicationCase.ToCreate, CreateModel.WrongfulMedicationCase.ToCreate>, WrongfulMedicationCaseToCreateMapper>();

        services.AddTransient<IMapper<EditModel.WrongfulRemovalCase.ToUpdate, CreateModel.WrongfulRemovalCase.ToUpdate>, WrongfulRemovalCaseToUpdateMapper>();
        services.AddTransient<IMapper<EditModel.WrongfulRemovalCase.ToCreate, CreateModel.WrongfulRemovalCase.ToCreate>, WrongfulRemovalCaseToCreateMapper>();

    }
}
