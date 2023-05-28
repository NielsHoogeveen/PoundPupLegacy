using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.EditModel.Readers;

public static class DependencyInjection
{
    public static void AddEditModelReaders(this IServiceCollection services)
    {
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, AbuseCase.NewAbuseCase>, AbuseCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, BlogPost.NewBlogPost>, BlogPostCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, ChildTraffickingCase.NewChildTraffickingCase>, ChildTraffickingCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, CoercedAdoptionCase.NewCoercedAdoptionCase>, CoercedAdoptionCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, DeportationCase.NewDeportationCase>, DeportationCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Discussion.NewDiscussion>, DiscussionCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, DisruptedPlacementCase.NewDisruptedPlacementCase>, DisruptedPlacementCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Document.NewDocument>, DocumentCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, FathersRightsViolationCase.NewFathersRightsViolationCase>, FathersRightsViolationCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Organization.NewOrganization>, OrganizationCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Person.NewPerson>, PersonCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, WrongfulMedicationCase.NewWrongfulMedicationCase>, WrongfulMedicationCaseCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, WrongfulRemovalCase.NewWrongfulRemovalCase>, WrongfulRemovalCaseCreateDocumentReaderFactory>();

        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, AbuseCase.ExistingAbuseCase>, AbuseCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, BlogPost.ExistingBlogPost>, BlogPostUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ChildTraffickingCase.ExistingChildTraffickingCase>, ChildTraffickingCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, CoercedAdoptionCase.ExistingCoercedAdoptionCase>, CoercedAdoptionCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, DeportationCase.ExistingDeportationCase>, DeportationCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Discussion.ExistingDiscussion>, DiscussionUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, DisruptedPlacementCase.ExistingDisruptedPlacementCase>, DisruptedPlacementCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Document.ExistingDocument>, DocumentUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, FathersRightsViolationCase.ExistingFathersRightsViolationCase>, FathersRightsViolationCaseUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Organization.ExistingOrganization>, OrganizationUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Person.ExistingPerson>, PersonUpdateDocumentReaderFactory>();
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
