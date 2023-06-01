namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class WrongfulRemovalCaseToUpdateMapper(
    IMapper<EditModel.NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailsMapper,
    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForUpdate> nameableDetailsMapper,
    IMapper<EditModel.LocatableDetails.ForUpdate, CreateModel.LocatableDetails.ForUpdate> locatableMapper,
    IMapper<EditModel.CaseDetails, CreateModel.CaseDetails.CaseDetailsForUpdate> caseDetailsMapper
    ) : IMapper<EditModel.WrongfulRemovalCase.ToUpdate, CreateModel.WrongfulRemovalCase.ToUpdate>
{
    public CreateModel.WrongfulRemovalCase.ToUpdate Map(EditModel.WrongfulRemovalCase.ToUpdate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.WrongfulRemovalCase.ToUpdate {
            Identification = new Identification.Certain {
                Id = viewModel.NodeIdentification.NodeId
            },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForUpdate),
            NameableDetails = nameableDetailsMapper.Map(viewModel.NameableDetails),
            CaseDetails = caseDetailsMapper.Map(viewModel.CaseDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsToUpdate),
        };
    }
}
