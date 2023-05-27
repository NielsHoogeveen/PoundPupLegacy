global using PoundPupLegacy.Common;
using PoundPupLegacy.CreateModel.Updaters;
using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.EditModel.UI.Services;
using PoundPupLegacy.EditModel.UI.Services.Implementation;

namespace PoundPupLegacy.EditModel.UI;

public static class DependencyInjection
{
    public static void AddEditModels(this IServiceCollection services)
    {
        services.AddEditModelReaders();
        services.AddEditModelInserters();
        services.AddCreateModelUpdaters();
        services.AddTransient<IAttachmentStoreService, AttachmentStoreService>();
        services.AddTransient<IEditService<AbuseCase, AbuseCase>, AbuseCaseEditService>();
        services.AddTransient<IEditService<BlogPost, BlogPost>, BlogPostEditService>();
        services.AddTransient<IEditService<ChildTraffickingCase, ResolvedChildTraffickingCase>, ChildTraffickingCaseEditService>();
        services.AddTransient<IEditService<CoercedAdoptionCase, CoercedAdoptionCase>, CoercedAdoptionCaseEditService>();
        services.AddTransient<IEditService<DeportationCase, DeportationCase>, DeportationCaseEditService>();
        services.AddTransient<IEditService<Discussion, Discussion>, DiscussionEditService>();
        services.AddTransient<IEditService<DisruptedPlacementCase, DisruptedPlacementCase>, DisruptedPlacementCaseEditService>();
        services.AddTransient<IEditService<Document, Document>, DocumentEditService>();
        services.AddTransient<IEditService<FathersRightsViolationCase, FathersRightsViolationCase>, FathersRightsViolationCaseEditService>();
        services.AddTransient<IEditService<Organization, Organization>, OrganizationEditService>();
        services.AddTransient<IEditService<Person, Person>, PersonEditService>();
        services.AddTransient<IEditService<WrongfulMedicationCase, WrongfulMedicationCase>, WrongfulMedicationCaseEditService>();
        services.AddTransient<IEditService<WrongfulRemovalCase, WrongfulRemovalCase>, WrongfulRemovalCaseEditService>();

        services.AddTransient<ILocationService, LocationService>();
        services.AddTransient<ISaveService<IEnumerable<ResolvedInterOrganizationalRelationFrom>>, InterOrganizationalRelationFromSaveService>();
        services.AddTransient<ISaveService<IEnumerable<ResolvedInterOrganizationalRelationTo>>, InterOrganizationalRelationToSaveService>();
        services.AddTransient<ISaveService<IEnumerable<ResolvedInterPersonalRelationFrom>>, InterPersonalRelationFromSaveService>();
        services.AddTransient<ISaveService<IEnumerable<ResolvedInterPersonalRelationTo>>, InterPersonalRelationToSaveService>();
        services.AddTransient<ISaveService<IEnumerable<OrganizationPoliticalEntityRelation>>, OrganizationPoliticalEntityRelationSaveService>();
        services.AddTransient<ISaveService<IEnumerable<PersonOrganizationRelation>>, PersonOrganizationRelationSaveService>();
        services.AddTransient<ISaveService<IEnumerable<File>>, FilesSaveService>();
        services.AddTransient<ITextService, TextService>();
        services.AddTransient<ISearchService<DocumentListItem>, DocumentSearchService>();
        services.AddTransient<ISearchService<GeographicalEntityListItem>, GeographicalEntitySearchService>();
        services.AddTransient<ISearchService<OrganizationListItem>, OrganizationSearchService>();
        services.AddTransient<ISearchService<PersonListItem>, PersonSearchService>();
        services.AddTransient<ISearchService<PoliticalEntityListItem>, PoliticalEntitySearchService>();
        services.AddTransient<ITopicSearchService, TopicSearchService>();
    }
}
