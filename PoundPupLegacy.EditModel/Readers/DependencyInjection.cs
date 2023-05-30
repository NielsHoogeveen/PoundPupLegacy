using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.EditModel.Readers;

public static class DependencyInjection
{
    public static void AddEditModelReaders(this IServiceCollection services)
    {
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, AbuseCase.ToCreate>, AbuseCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, BlogPost.ToCreate>, BlogPostCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, ChildTraffickingCase.ToCreate.Unresolved>, ChildTraffickingCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, CoercedAdoptionCase.ToCreate>, CoercedAdoptionCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, DeportationCase.ToCreate>, DeportationCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Discussion.ToCreate>, DiscussionCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, DisruptedPlacementCase.ToCreate>, DisruptedPlacementCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Document.ToCreate>, DocumentCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, FathersRightsViolationCase.ToCreate>, FathersRightsViolationCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Organization.ToCreate>, OrganizationCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Person.ToCreate>, PersonCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, WrongfulMedicationCase.NewWrongfulMedicationCase>, WrongfulMedicationCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, WrongfulRemovalCase.NewWrongfulRemovalCase>, WrongfulRemovalCaseCreateDocumentReaderFactory>();

        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, AbuseCase.ToUpdate>, AbuseCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, BlogPost.ToUpdate>, BlogPostUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ChildTraffickingCase.ToUpdate>, ChildTraffickingCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, CoercedAdoptionCase.ToUpdate>, CoercedAdoptionCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, DeportationCase.ToUpdate>, DeportationCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Discussion.ToUpdate>, DiscussionUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, DisruptedPlacementCase.ToUpdate>, DisruptedPlacementCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Document.ToUpdate>, DocumentUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, FathersRightsViolationCase.ToUpdate>, FathersRightsViolationCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Organization.ToUpdate>, OrganizationUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Person.ToUpdate>, PersonUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, WrongfulMedicationCase.ExistingWrongfulMedicationCase>, WrongfulMedicationCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, WrongfulRemovalCase.ExistingWrongfulRemovalCase>, WrongfulRemovalCaseUpdateDocumentReaderFactory>();

        services.AddTransient<IEnumerableDatabaseReaderFactory<CountryListItemsReaderRequest, CountryListItem>, CountryListItemsReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<GeographicalEntitiesReaderRequest, GeographicalEntityListItem>, GeographicalEntitiesReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<DocumentsReaderRequest, DocumentListItem>, DocumentsReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<OrganizationsReaderRequest, OrganizationListItem>, OrganizationsReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<PersonsReaderRequest, PersonListItem>, PersonsReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<PoliticalEntitiesReaderRequest, PoliticalEntityListItem>, PoliticalEntitiesReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<SubdivisionListItemsReaderRequest, SubdivisionListItem>, SubdivisionListItemsReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<TagDocumentsReaderRequest, NodeTerm.NewNodeTerm>, TagDocumentsReaderFactory>();
        services.AddTransient<IDoesRecordExistDatabaseReaderFactory<TopicExistsRequest>, TopicExistsFactory>();
    }
}
