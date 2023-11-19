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
        services.AddViewModelRetrieveService<BasicNameable, BasicNameable.ToUpdate, BasicNameable.ToCreate>();
        services.AddViewModelRetrieveService<BlogPost, BlogPost.ToUpdate, BlogPost.ToCreate>();
        services.AddViewModelRetrieveService<UnresolvedChildTraffickingCase, UnresolvedChildTraffickingCase.ToUpdate, UnresolvedChildTraffickingCase.ToCreate>();
        services.AddViewModelRetrieveService<Denomination, Denomination.ToUpdate, Denomination.ToCreate>();
        services.AddViewModelRetrieveService<ChildPlacementType, ChildPlacementType.ToUpdate, ChildPlacementType.ToCreate>();
        services.AddViewModelRetrieveService<CoercedAdoptionCase, CoercedAdoptionCase.ToUpdate, CoercedAdoptionCase.ToCreate>();
        services.AddViewModelRetrieveService<Discussion, Discussion.ToUpdate, Discussion.ToCreate>();
        services.AddViewModelRetrieveService<DeportationCase, DeportationCase.ToUpdate, DeportationCase.ToCreate>();
        services.AddViewModelRetrieveService<Document, Document.ToUpdate, Document.ToCreate>();
        services.AddViewModelRetrieveService<DocumentType, DocumentType.ToUpdate, DocumentType.ToCreate>();
        services.AddViewModelRetrieveService<DisruptedPlacementCase, DisruptedPlacementCase.ToUpdate, DisruptedPlacementCase.ToCreate>();
        services.AddViewModelRetrieveService<FathersRightsViolationCase, FathersRightsViolationCase.ToUpdate, FathersRightsViolationCase.ToCreate>();
        services.AddViewModelRetrieveService<HagueStatus, HagueStatus.ToUpdate, HagueStatus.ToCreate>();
        services.AddViewModelRetrieveService<InterOrganizationalRelationType, InterOrganizationalRelationType.ToUpdate, InterOrganizationalRelationType.ToCreate>();
        services.AddViewModelRetrieveService<InterPersonalRelationType, InterPersonalRelationType.ToUpdate, InterPersonalRelationType.ToCreate>();
        services.AddViewModelRetrieveService<PartyPoliticalEntityRelationType, PartyPoliticalEntityRelationType.ToUpdate, PartyPoliticalEntityRelationType.ToCreate>();
        services.AddViewModelRetrieveService<PersonOrganizationRelationType, PersonOrganizationRelationType.ToUpdate, PersonOrganizationRelationType.ToCreate>();
        services.AddViewModelRetrieveService<Profession, Profession.ToUpdate, Profession.ToCreate>();
        services.AddViewModelRetrieveService<TypeOfAbuse, TypeOfAbuse.ToUpdate, TypeOfAbuse.ToCreate>();
        services.AddViewModelRetrieveService<TypeOfAbuser, TypeOfAbuser.ToUpdate, TypeOfAbuser.ToCreate>();
        services.AddViewModelRetrieveService<WrongfulMedicationCase, WrongfulMedicationCase.ToUpdate, WrongfulMedicationCase.ToCreate>();
        services.AddViewModelRetrieveService<WrongfulRemovalCase, WrongfulRemovalCase.ToUpdate, WrongfulRemovalCase.ToCreate>();
        services.AddViewModelRetrieveService<Organization, Organization.ToUpdate, Organization.ToCreate>();
        services.AddViewModelRetrieveService<OrganizationType, OrganizationType.ToUpdate, OrganizationType.ToCreate>();
        services.AddViewModelRetrieveService<Person, Person.ToUpdate, Person.ToCreate>();
        services.AddViewModelRetrieveService<UnitedStatesCity, UnitedStatesCity.ToUpdate, UnitedStatesCity.ToCreate>();

        services.AddEntitySaveService<
            AbuseCase, 
            AbuseCase.ToUpdate, 
            AbuseCase.ToCreate,
            DomainModel.AbuseCase,
            DomainModel.AbuseCase.ToUpdate,
            DomainModel.AbuseCase.ToCreate>();

        services.AddEntitySaveService<
            BasicNameable,
            BasicNameable.ToUpdate,
            BasicNameable.ToCreate,
            DomainModel.BasicNameable,
            DomainModel.BasicNameable.ToUpdate,
            DomainModel.BasicNameable.ToCreate>();

        services.AddEntitySaveService<
            BlogPost,
            BlogPost.ToUpdate,
            BlogPost.ToCreate,
            DomainModel.BlogPost,
            DomainModel.BlogPost.ToUpdate,
            DomainModel.BlogPost.ToCreate>();

        services.AddEntitySaveService<
            ChildPlacementType,
            ChildPlacementType.ToUpdate,
            ChildPlacementType.ToCreate,
            DomainModel.ChildPlacementType,
            DomainModel.ChildPlacementType.ToUpdate,
            DomainModel.ChildPlacementType.ToCreate>();

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
            Denomination,
            Denomination.ToUpdate,
            Denomination.ToCreate,
            DomainModel.Denomination,
            DomainModel.Denomination.ToUpdate,
            DomainModel.Denomination.ToCreate>();

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
            DocumentType,
            DocumentType.ToUpdate,
            DocumentType.ToCreate,
            DomainModel.DocumentType,
            DomainModel.DocumentType.ToUpdate,
            DomainModel.DocumentType.ToCreate>();

        services.AddEntitySaveService<
            FathersRightsViolationCase,
            FathersRightsViolationCase.ToUpdate,
            FathersRightsViolationCase.ToCreate,
            DomainModel.FathersRightsViolationCase,
            DomainModel.FathersRightsViolationCase.ToUpdate,
            DomainModel.FathersRightsViolationCase.ToCreate>();

        services.AddEntitySaveService<
            HagueStatus,
            HagueStatus.ToUpdate,
            HagueStatus.ToCreate,
            DomainModel.HagueStatus,
            DomainModel.HagueStatus.ToUpdate,
            DomainModel.HagueStatus.ToCreate>();

        services.AddEntitySaveService<
            InterOrganizationalRelationType,
            InterOrganizationalRelationType.ToUpdate,
            InterOrganizationalRelationType.ToCreate,
            DomainModel.InterOrganizationalRelationType,
            DomainModel.InterOrganizationalRelationType.ToUpdate,
            DomainModel.InterOrganizationalRelationType.ToCreate>();

        services.AddEntitySaveService<
            InterPersonalRelationType,
            InterPersonalRelationType.ToUpdate,
            InterPersonalRelationType.ToCreate,
            DomainModel.InterPersonalRelationType,
            DomainModel.InterPersonalRelationType.ToUpdate,
            DomainModel.InterPersonalRelationType.ToCreate>();

        services.AddEntitySaveService<
            Organization,
            Organization.ToUpdate,
            Organization.ToCreate,
            DomainModel.Organization,
            OrganizationToUpdate,
            OrganizationToCreate>();

        services.AddEntitySaveService<
            OrganizationType,
            OrganizationType.ToUpdate,
            OrganizationType.ToCreate,
            DomainModel.OrganizationType,
            DomainModel.OrganizationType.ToUpdate,
            DomainModel.OrganizationType.ToCreate>();

        services.AddEntitySaveService<
            PartyPoliticalEntityRelationType,
            PartyPoliticalEntityRelationType.ToUpdate,
            PartyPoliticalEntityRelationType.ToCreate,
            DomainModel.PartyPoliticalEntityRelationType,
            DomainModel.PartyPoliticalEntityRelationType.ToUpdate,
            DomainModel.PartyPoliticalEntityRelationType.ToCreate>();

        services.AddEntitySaveService<
            Person,
            Person.ToUpdate,
            Person.ToCreate,
            DomainModel.Person,
            DomainModel.Person.ToUpdate,
            DomainModel.Person.ToCreate>();

        services.AddEntitySaveService<
            PersonOrganizationRelationType,
            PersonOrganizationRelationType.ToUpdate,
            PersonOrganizationRelationType.ToCreate,
            DomainModel.PersonOrganizationRelationType,
            DomainModel.PersonOrganizationRelationType.ToUpdate,
            DomainModel.PersonOrganizationRelationType.ToCreate>();

        services.AddEntitySaveService<
            Profession,
            Profession.ToUpdate,
            Profession.ToCreate,
            DomainModel.Profession,
            DomainModel.Profession.ToUpdate,
            DomainModel.Profession.ToCreate>();

        services.AddEntitySaveService<
            TypeOfAbuse,
            TypeOfAbuse.ToUpdate,
            TypeOfAbuse.ToCreate,
            DomainModel.TypeOfAbuse,
            DomainModel.TypeOfAbuse.ToUpdate,
            DomainModel.TypeOfAbuse.ToCreate>();

        services.AddEntitySaveService<
            TypeOfAbuser,
            TypeOfAbuser.ToUpdate,
            TypeOfAbuser.ToCreate,
            DomainModel.TypeOfAbuser,
            DomainModel.TypeOfAbuser.ToUpdate,
            DomainModel.TypeOfAbuser.ToCreate>();

        services.AddEntitySaveService<
            UnitedStatesCity,
            UnitedStatesCity.ToUpdate,
            UnitedStatesCity.ToCreate,
            DomainModel.UnitedStatesCity,
            DomainModel.UnitedStatesCity.ToUpdate,
            DomainModel.UnitedStatesCity.ToCreate>();

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
        services.AddTransient<ISearchService<UnitedStatesCountyListItem>, UnitedStatesCountySearchService>();
        services.AddTransient<ISearchService<GeographicalEntityListItem>, GeographicalEntitySearchService>();
        services.AddTransient<ISearchService<OrganizationListItem>, OrganizationSearchService>();
        services.AddTransient<ISearchService<PersonListItem>, PersonSearchService>();
        services.AddTransient<ISearchService<PoliticalEntityListItem>, PoliticalEntitySearchService>();
        services.AddTransient<ITopicSearchService, TopicSearchService>();
    }
}
