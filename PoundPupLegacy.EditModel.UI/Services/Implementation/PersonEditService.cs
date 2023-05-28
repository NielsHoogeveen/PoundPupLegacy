//using Microsoft.Extensions.Logging;
//using PoundPupLegacy.CreateModel;

//namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

//internal sealed class PersonEditService(
//    IDbConnection connection,
//    ILogger<PersonEditService> logger,
//    ITenantRefreshService tenantRefreshService,
//    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewPerson> createViewModelReaderFactory,
//    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingPerson> updateViewModelReaderFactory,
//    IDatabaseUpdaterFactory<ImmediatelyIdentifiablePerson> updaterFactory,
//    IEntityCreatorFactory<EventuallyIdentifiablePerson> creatorFactory,
//    ITextService textService

//) : NodeEditServiceBase<
//    Person,
//    Person,
//    ExistingPerson,
//    NewPerson,
//    NewPerson,
//    CreateModel.Person,
//    EventuallyIdentifiablePerson,
//    ImmediatelyIdentifiablePerson>(
//    connection,
//    logger,
//    tenantRefreshService,
//    creatorFactory,
//    updaterFactory,
//    createViewModelReaderFactory,
//    updateViewModelReaderFactory
//), IEditService<Person, Person>
//{
//    protected override ImmediatelyIdentifiablePerson Map(ExistingPerson existingViewModel)
//    {
//        throw new NotImplementedException();
//    }

//    protected override EventuallyIdentifiablePerson Map(NewPerson newViewModel)
//    {
//        throw new NotImplementedException();
//    }
//}
