//using Microsoft.Extensions.Logging;
//using PoundPupLegacy.CreateModel;
//using PoundPupLegacy.CreateModel.Deleters;

//namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

//internal sealed class DisruptedPlacementCaseEditService(
//    IDbConnection connection,
//    ILogger<DisruptedPlacementCaseEditService> logger,
//    ITenantRefreshService tenantRefreshService,
//    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDisruptedPlacementCase> createViewModelReaderFactory,
//    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDisruptedPlacementCase> updateViewModelReaderFactory,
//    IEntityCreatorFactory<EventuallyIdentifiableDisruptedPlacementCase> creatorFactory,
//    IDatabaseUpdaterFactory<ImmediatelyIdentifiableDisruptedPlacementCase> updaterFactory,
//    ITextService textService
//) : NodeEditServiceBase<
//        EditModel.DisruptedPlacementCase,
//        DisruptedPlacementCase,
//        ExistingDisruptedPlacementCase,
//        NewDisruptedPlacementCase,
//        NewDisruptedPlacementCase,
//        CreateModel.DisruptedPlacementCase,
//        EventuallyIdentifiableDisruptedPlacementCase,
//        ImmediatelyIdentifiableDisruptedPlacementCase>
//(
//    connection,
//    logger,
//    tenantRefreshService,
//    creatorFactory,
//    updaterFactory,
//    createViewModelReaderFactory,
//    updateViewModelReaderFactory
//), IEditService<DisruptedPlacementCase, DisruptedPlacementCase>
//{

//    protected sealed override EventuallyIdentifiableDisruptedPlacementCase Map(NewDisruptedPlacementCase viewModel)
//    {
//        var now = DateTime.Now;
//        return new CreateModel.NewDisruptedPlacementCase {
//            Id = null,
//            Title = viewModel.Title,
//            Description = viewModel.Description is null ? "" : textService.FormatText(viewModel.Description),
//            ChangedDateTime = now,
//            CreatedDateTime = now,
//            NodeTypeId = Constants.DOCUMENT,
//            OwnerId = viewModel.OwnerId,
//            AuthoringStatusId = 1,
//            PublisherId = viewModel.PublisherId,
//            TenantNodes = viewModel.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.NewTenantNodeForNewNode {
//                Id = null,
//                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
//                TenantId = tn.TenantNode!.TenantId,
//                UrlId = null,
//                UrlPath = tn.TenantNode!.UrlPath,
//                SubgroupId = tn.TenantNode!.SubgroupId,
//            }).ToList(),
//            Date = viewModel.Date,
//            FileIdTileImage = null,
//            Terms = new List<NewTermForNewNameable>(),
//            TermIds = new List<int>(),
//            Locations = new List<EventuallyIdentifiableLocation>(),
//            CaseParties = new List<NewCaseNewCaseParties>(),
//        };
//    }

//    protected sealed override ImmediatelyIdentifiableDisruptedPlacementCase Map(ExistingDisruptedPlacementCase viewModel)
//    {
//        return new CreateModel.ExistingDisruptedPlacementCase {
//            Id = viewModel.NodeId,
//            Title = viewModel.Title,
//            Description = viewModel.Description is null ? "" : textService.FormatText(viewModel.Description),
//            ChangedDateTime = DateTime.Now,
//            AuthoringStatusId = 1,
//            Date = viewModel.Date,
//            FileIdTileImage = null,
//            NodeTermsToAdd = new List<NodeTermToAdd>(),
//            TenantNodesToAdd = new List<NewTenantNodeForExistingNode>(),
//            NodeTermsToRemove = new List<NodeTermToRemove>(),
//            TenantNodesToRemove = new List<TenantNodeToDelete>(),
//            TenantNodesToUpdate = new List<CreateModel.ExistingTenantNode>(),
//            TermsToAdd = new List<NewTermForExistingNameable>(),
//            LocationsToDelete = new List<int>(),
//            LocationsToUpdate = new List<ImmediatelyIdentifiableLocation>(),
//            LocationsToAdd = new List<EventuallyIdentifiableLocation>(),
//            CasePartiesToAdd = new List<ExistingCaseNewCaseParties>(),
//            CasePartiesToUpdate = new List<ExistingCaseExistingCaseParties>(),
//        };
//    }
//}
