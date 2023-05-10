using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.EditModel.Readers;

public static class DependencyInjection
{
    public static void AddEditModelReaders(this IServiceCollection services)
    {
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, AbuseCase>, AbuseCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, BlogPost>, BlogPostCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, ChildTraffickingCase>, ChildTraffickingCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, CoercedAdoptionCase>, CoercedAdoptionCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, DeportationCase>, DeportationCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Discussion>, DiscussionCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, DisruptedPlacementCase>, DisruptedPlacementCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Document>, DocumentCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, FathersRightsViolationCase>, FathersRightsViolationCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Organization>, OrganizationCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Person>, PersonCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, WrongfulMedicationCase>, WrongfulMedicationCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, WrongfulRemovalCase>, WrongfulRemovalCaseCreateDocumentReaderFactory>();
        
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, AbuseCase>, AbuseCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, BlogPost>, BlogPostUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ChildTraffickingCase>, ChildTraffickingCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, CoercedAdoptionCase>, CoercedAdoptionCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, DeportationCase>, DeportationCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Discussion>, DiscussionUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, DisruptedPlacementCase>, DisruptedPlacementCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Document>, DocumentUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, FathersRightsViolationCase>, FathersRightsViolationCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Organization>, OrganizationUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Person>, PersonUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, WrongfulMedicationCase>, WrongfulMedicationCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, WrongfulRemovalCase>, WrongfulRemovalCaseUpdateDocumentReaderFactory>();

        services.AddTransient<IEnumerableDatabaseReaderFactory<CountryListItemsReaderRequest, CountryListItem>, CountryListItemsReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<GeographicalEntitiesReaderRequest, GeographicalEntityListItem>, GeographicalEntitiesReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<DocumentsReaderRequest, DocumentListItem>, DocumentsReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<OrganizationsReaderRequest, OrganizationListItem>, OrganizationsReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<PersonsReaderRequest, PersonListItem>, PersonsReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<SubdivisionListItemsReaderRequest, SubdivisionListItem>, SubdivisionListItemsReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<TagDocumentsReaderRequest, Tag>, TagDocumentsReaderFactory>();
        services.AddTransient<IDoesRecordExistDatabaseReaderFactory<TopicExistsRequest>, TopicExistsFactory>();
    }
}
