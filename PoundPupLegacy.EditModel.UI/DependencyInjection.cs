global using PoundPupLegacy.Common;
using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.EditModel.UI.Services;
using PoundPupLegacy.EditModel.UI.Services.Implementation;
using PoundPupLegacy.EditModel.Mappers;
using PoundPupLegacy.DomainModel.Updaters;
using PoundPupLegacy.DomainModel;
using PoundPupLegacy.DomainModel.Deleters;

namespace PoundPupLegacy.EditModel.UI;

public static class DependencyInjection
{
    public static void AddEditModels(this IServiceCollection services)
    {
        services.AddEditModelReaders();
        services.AddEditModelInserters();
        services.AddDomainModelUpdaters();
        services.AddDomainModelDeleters();
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
            DomainModel.AbuseCase,
            DomainModel.AbuseCase.ToUpdate,
            DomainModel.AbuseCase.ToCreate>();

        services.AddEntitySaveService<
            BlogPost,
            BlogPost.ToUpdate,
            BlogPost.ToCreate,
            DomainModel.BlogPost,
            DomainModel.BlogPost.ToUpdate,
            DomainModel.BlogPost.ToCreate>();

        services.AddEntitySaveService<
            ChildTraffickingCase,
            ChildTraffickingCase.ToUpdate,
            ChildTraffickingCase.ToCreate,
            DomainModel.ChildTraffickingCase,
            DomainModel.ChildTraffickingCase.ToUpdate,
            DomainModel.ChildTraffickingCase.ToCreate>();

        services.AddEntitySaveService<
            CoercedAdoptionCase,
            CoercedAdoptionCase.ToUpdate,
            CoercedAdoptionCase.ToCreate,
            DomainModel.CoercedAdoptionCase,
            DomainModel.CoercedAdoptionCase.ToUpdate,
            DomainModel.CoercedAdoptionCase.ToCreate>();

        services.AddEntitySaveService<
            DeportationCase,
            DeportationCase.ToUpdate,
            DeportationCase.ToCreate,
            DomainModel.DeportationCase,
            DomainModel.DeportationCase.ToUpdate,
            DomainModel.DeportationCase.ToCreate>();

        services.AddEntitySaveService<
            Discussion,
            Discussion.ToUpdate,
            Discussion.ToCreate,
            DomainModel.Discussion,
            DomainModel.Discussion.ToUpdate,
            DomainModel.Discussion.ToCreate>();

        services.AddEntitySaveService<
            DisruptedPlacementCase,
            DisruptedPlacementCase.ToUpdate,
            DisruptedPlacementCase.ToCreate,
            DomainModel.DisruptedPlacementCase,
            DomainModel.DisruptedPlacementCase.ToUpdate,
            DomainModel.DisruptedPlacementCase.ToCreate>();

        services.AddEntitySaveService<
            Document,
            Document.ToUpdate,
            Document.ToCreate,
            DomainModel.Document,
            DomainModel.Document.ToUpdate,
            DomainModel.Document.ToCreate>();

        services.AddEntitySaveService<
            FathersRightsViolationCase,
            FathersRightsViolationCase.ToUpdate,
            FathersRightsViolationCase.ToCreate,
            DomainModel.FathersRightsViolationCase,
            DomainModel.FathersRightsViolationCase.ToUpdate,
            DomainModel.FathersRightsViolationCase.ToCreate>();

        services.AddEntitySaveService<
            Organization,
            Organization.ToUpdate,
            Organization.ToCreate,
            DomainModel.Organization,
            OrganizationToUpdate,
            OrganizationToCreate>();

        services.AddEntitySaveService<
            Person,
            Person.ToUpdate,
            Person.ToCreate,
            DomainModel.Person,
            DomainModel.Person.ToUpdate,
            DomainModel.Person.ToCreate>();

        services.AddEntitySaveService<
            WrongfulMedicationCase,
            WrongfulMedicationCase.ToUpdate,
            WrongfulMedicationCase.ToCreate,
            DomainModel.WrongfulMedicationCase,
            DomainModel.WrongfulMedicationCase.ToUpdate,
            DomainModel.WrongfulMedicationCase.ToCreate>();

        services.AddEntitySaveService<
            WrongfulRemovalCase,
            WrongfulRemovalCase.ToUpdate,
            WrongfulRemovalCase.ToCreate,
            DomainModel.WrongfulRemovalCase,
            DomainModel.WrongfulRemovalCase.ToUpdate,
            DomainModel.WrongfulRemovalCase.ToCreate>();

        services.AddTransient<ILocationService, LocationService>();
        services.AddTransient<ISaveService<IEnumerable<File>>, FilesSaveService>();
        services.AddTransient<ITextService, TextService>();
        services.AddTransient<ISearchService<DocumentListItem>, DocumentSearchService>();
        services.AddTransient<ISearchService<CountryListItem>, CountrySearchService>();
        services.AddTransient<ISearchService<GeographicalEntityListItem>, GeographicalEntitySearchService>();
        services.AddTransient<ISearchService<OrganizationListItem>, OrganizationSearchService>();
        services.AddTransient<ISearchService<PersonListItem>, PersonSearchService>();
        services.AddTransient<ISearchService<PoliticalEntityListItem>, PoliticalEntitySearchService>();
        services.AddTransient<ITopicSearchService, TopicSearchService>();
    }
}
