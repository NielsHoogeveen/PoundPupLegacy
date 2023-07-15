global using PoundPupLegacy.Common;
using PoundPupLegacy.CreateModel.Updaters;
using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.EditModel.UI.Services;
using PoundPupLegacy.EditModel.UI.Services.Implementation;
using PoundPupLegacy.CreateModel.Deleters;
using PoundPupLegacy.EditModel.Mappers;

namespace PoundPupLegacy.EditModel.UI;

public static class DependencyInjection
{
    public static void AddEditModels(this IServiceCollection services)
    {
        services.AddEditModelReaders();
        services.AddEditModelInserters();
        services.AddCreateModelUpdaters();
        services.AddCreateModelDeleters();
        services.AddMappers();
        services.AddTransient<IAttachmentStoreService, AttachmentStoreService>();
        services.AddViewModelRetrieveService<AbuseCase, AbuseCase.ToUpdate, AbuseCase.ToCreate>();
        services.AddViewModelRetrieveService<BlogPost, BlogPost.ToUpdate, BlogPost.ToCreate>();
        services.AddViewModelRetrieveService<UnresolvedChildTraffickingCase, UnresolvedChildTraffickingCase.ToUpdate, UnresolvedChildTraffickingCase.ToCreate>();
        services.AddViewModelRetrieveService<CoercedAdoptionCase, CoercedAdoptionCase.ToUpdate, CoercedAdoptionCase.ToCreate>();
        services.AddViewModelRetrieveService<Discussion, Discussion.ToUpdate, Discussion.ToCreate>();
        services.AddViewModelRetrieveService<DeportationCase, DeportationCase.ToUpdate, DeportationCase.ToCreate>();
        services.AddViewModelRetrieveService<Document, Document.ToUpdate, Document.ToCreate>();
        services.AddViewModelRetrieveService<DisruptedPlacementCase, DisruptedPlacementCase.ToUpdate, DisruptedPlacementCase.ToCreate>();
        services.AddViewModelRetrieveService<FathersRightsViolationCase, FathersRightsViolationCase.ToUpdate, FathersRightsViolationCase.ToCreate>();
        services.AddViewModelRetrieveService<WrongfulMedicationCase, WrongfulMedicationCase.ToUpdate, WrongfulMedicationCase.ToCreate>();
        services.AddViewModelRetrieveService<WrongfulRemovalCase, WrongfulRemovalCase.ToUpdate, WrongfulRemovalCase.ToCreate>();
        services.AddViewModelRetrieveService<Organization, Organization.ToUpdate, Organization.ToCreate>();
        services.AddViewModelRetrieveService<Person, Person.ToUpdate, Person.ToCreate>();

        services.AddEntitySaveService<
            AbuseCase, 
            AbuseCase.ToUpdate, 
            AbuseCase.ToCreate, 
            CreateModel.AbuseCase, 
            CreateModel.AbuseCase.ToUpdate, 
            CreateModel.AbuseCase.ToCreate>();

        services.AddEntitySaveService<
            BlogPost,
            BlogPost.ToUpdate,
            BlogPost.ToCreate,
            CreateModel.BlogPost,
            CreateModel.BlogPost.ToUpdate,
            CreateModel.BlogPost.ToCreate>();

        services.AddEntitySaveService<
            ChildTraffickingCase,
            ChildTraffickingCase.ToUpdate,
            ChildTraffickingCase.ToCreate,
            CreateModel.ChildTraffickingCase,
            CreateModel.ChildTraffickingCase.ToUpdate,
            CreateModel.ChildTraffickingCase.ToCreate>();

        services.AddEntitySaveService<
            CoercedAdoptionCase,
            CoercedAdoptionCase.ToUpdate,
            CoercedAdoptionCase.ToCreate,
            CreateModel.CoercedAdoptionCase,
            CreateModel.CoercedAdoptionCase.ToUpdate,
            CreateModel.CoercedAdoptionCase.ToCreate>();

        services.AddEntitySaveService<
            DeportationCase,
            DeportationCase.ToUpdate,
            DeportationCase.ToCreate,
            CreateModel.DeportationCase,
            CreateModel.DeportationCase.ToUpdate,
            CreateModel.DeportationCase.ToCreate>();

        services.AddEntitySaveService<
            Discussion,
            Discussion.ToUpdate,
            Discussion.ToCreate,
            CreateModel.Discussion,
            CreateModel.Discussion.ToUpdate,
            CreateModel.Discussion.ToCreate>();

        services.AddEntitySaveService<
            DisruptedPlacementCase,
            DisruptedPlacementCase.ToUpdate,
            DisruptedPlacementCase.ToCreate,
            CreateModel.DisruptedPlacementCase,
            CreateModel.DisruptedPlacementCase.ToUpdate,
            CreateModel.DisruptedPlacementCase.ToCreate>();

        services.AddEntitySaveService<
            Document,
            Document.ToUpdate,
            Document.ToCreate,
            CreateModel.Document,
            CreateModel.Document.ToUpdate,
            CreateModel.Document.ToCreate>();

        services.AddEntitySaveService<
            FathersRightsViolationCase,
            FathersRightsViolationCase.ToUpdate,
            FathersRightsViolationCase.ToCreate,
            CreateModel.FathersRightsViolationCase,
            CreateModel.FathersRightsViolationCase.ToUpdate,
            CreateModel.FathersRightsViolationCase.ToCreate>();

        services.AddEntitySaveService<
            Organization,
            Organization.ToUpdate,
            Organization.ToCreate,
            CreateModel.Organization,
            CreateModel.OrganizationToUpdate,
            CreateModel.OrganizationToCreate>();

        services.AddEntitySaveService<
            Person,
            Person.ToUpdate,
            Person.ToCreate,
            CreateModel.Person,
            CreateModel.Person.ToUpdate,
            CreateModel.Person.ToCreate>();

        services.AddEntitySaveService<
            WrongfulMedicationCase,
            WrongfulMedicationCase.ToUpdate,
            WrongfulMedicationCase.ToCreate,
            CreateModel.WrongfulMedicationCase,
            CreateModel.WrongfulMedicationCase.ToUpdate,
            CreateModel.WrongfulMedicationCase.ToCreate>();

        services.AddEntitySaveService<
            WrongfulRemovalCase,
            WrongfulRemovalCase.ToUpdate,
            WrongfulRemovalCase.ToCreate,
            CreateModel.WrongfulRemovalCase,
            CreateModel.WrongfulRemovalCase.ToUpdate,
            CreateModel.WrongfulRemovalCase.ToCreate>();

        services.AddTransient<ILocationService, LocationService>();
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
