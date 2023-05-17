global using PoundPupLegacy.Common;
using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.EditModel.UI.Services;
using PoundPupLegacy.EditModel.UI.Services.Implementation;

namespace PoundPupLegacy.EditModel.UI;

public static class DependencyInjection
{
    public static void AddEditModels(this IServiceCollection services)
    {
        services.AddEditModelReaders();
        services.AddEditModelDeleters();
        services.AddEditModelInserters();
        services.AddEditModelUpdaters();
        services.AddTransient<IAttachmentStoreService, AttachmentStoreService>();
        services.AddTransient<IEditService<AbuseCase>, AbuseCaseEditService>();
        services.AddTransient<IEditService<BlogPost>, BlogPostEditService>();
        services.AddTransient<IEditService<ChildTraffickingCase>, ChildTraffickingCaseEditService>();
        services.AddTransient<IEditService<CoercedAdoptionCase>, CoercedAdoptionCaseEditService>();
        services.AddTransient<IEditService<DeportationCase>, DeportationCaseEditService>();
        services.AddTransient<IEditService<Discussion>, DiscussionEditService>();
        services.AddTransient<IEditService<DisruptedPlacementCase>, DisruptedPlacementCaseEditService>();
        services.AddTransient<IEditService<Document>, DocumentEditService>();
        services.AddTransient<IEditService<FathersRightsViolationCase>, FathersRightsViolationCaseEditService>();
        services.AddTransient<IEditService<Organization>, OrganizationEditService>();
        services.AddTransient<IEditService<Person>, PersonEditService>();
        services.AddTransient<IEditService<WrongfulMedicationCase>, WrongfulMedicationCaseEditService>();
        services.AddTransient<IEditService<WrongfulRemovalCase>, WrongfulRemovalCaseEditService>();

        services.AddTransient<ILocationService, LocationService>();
        services.AddTransient<ISaveService<IEnumerable<ResolvedInterOrganizationalRelationFrom>>, InterOrganizationalRelationFromSaveService>();
        services.AddTransient<ISaveService<IEnumerable<ResolvedInterOrganizationalRelationTo>>, InterOrganizationalRelationToSaveService>();
        services.AddTransient<ISaveService<IEnumerable<InterPersonalRelation>>, InterPersonalRelationSaveService>();
        services.AddTransient<ISaveService<IEnumerable<OrganizationPoliticalEntityRelation>>, OrganizationPoliticalEntityRelationSaveService>();
        services.AddTransient<ISaveService<IEnumerable<PersonOrganizationRelation>>, PersonOrganizationRelationSaveService>();
        services.AddTransient<ISaveService<IEnumerable<File>>, FilesSaveService>();
        services.AddTransient<ISaveService<IEnumerable<TenantNode>>, TenantNodesSaveService>();
        services.AddTransient<ISaveService<IEnumerable<Tag>>, TagsSaveService>();
        services.AddTransient<ISaveService<IEnumerable<Location>>, LocationsSaveService>();
        services.AddTransient<ITextService, TextService>();
        services.AddTransient<ISearchService<DocumentListItem>, DocumentSearchService>();
        services.AddTransient<ISearchService<GeographicalEntityListItem>, GeographicalEntitySearchService>();
        services.AddTransient<ISearchService<OrganizationListItem>, OrganizationSearchService>();
        services.AddTransient<ISearchService<PersonListItem>, PersonSearchService>();
        services.AddTransient<ISearchService<PoliticalEntityListItem>, PoliticalEntitySearchService>();
        services.AddTransient<ITopicSearchService, TopicSearchService>();
    }
}
