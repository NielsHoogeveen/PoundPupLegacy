using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.EditModel.Readers;

public static class DependencyInjection
{
    public static void AddEditModelReaders(this IServiceCollection services)
    {
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewAbuseCase>, AbuseCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewBlogPost>, BlogPostCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewChildTraffickingCase>, ChildTraffickingCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewCoercedAdoptionCase>, CoercedAdoptionCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDeportationCase>, DeportationCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDiscussion>, DiscussionCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDisruptedPlacementCase>, DisruptedPlacementCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDocument>, DocumentCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewFathersRightsViolationCase>, FathersRightsViolationCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewOrganization>, OrganizationCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewPerson>, PersonCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewWrongfulMedicationCase>, WrongfulMedicationCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewWrongfulRemovalCase>, WrongfulRemovalCaseCreateDocumentReaderFactory>();

        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingAbuseCase>, AbuseCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingBlogPost>, BlogPostUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingChildTraffickingCase>, ChildTraffickingCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingCoercedAdoptionCase>, CoercedAdoptionCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDeportationCase>, DeportationCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDiscussion>, DiscussionUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDisruptedPlacementCase>, DisruptedPlacementCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDocument>, DocumentUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingFathersRightsViolationCase>, FathersRightsViolationCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingOrganization>, OrganizationUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingPerson>, PersonUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingWrongfulMedicationCase>, WrongfulMedicationCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingWrongfulRemovalCase>, WrongfulRemovalCaseUpdateDocumentReaderFactory>();

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
