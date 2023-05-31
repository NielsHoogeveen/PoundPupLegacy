namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class AbusCaseUpdateMapper(
    IMapper<EditModel.NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailsMapper,
    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForUpdate> nameableDetailsMapper,
    IMapper<EditModel.LocatableDetails.ForUpdate, CreateModel.LocatableDetails.ForUpdate> locatableMapper,
    IMapper<EditModel.CaseDetails, CreateModel.CaseDetails.CaseDetailsForUpdate> caseDetailsMapper,
    IMapper<EditModel.AbuseCaseDetails, CreateModel.AbuseCaseDetails> abuseCaseDetailsMapper
    ) : IMapper<EditModel.AbuseCase.ToUpdate, CreateModel.AbuseCase.ToUpdate>
{
    public CreateModel.AbuseCase.ToUpdate Map(EditModel.AbuseCase.ToUpdate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.AbuseCase.ToUpdate {
            Identification = new Identification.Certain {
                Id = viewModel.NodeIdentification.NodeId
            },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForUpdate),
            NameableDetails = nameableDetailsMapper.Map(viewModel.NameableDetails),
            CaseDetails = caseDetailsMapper.Map(viewModel.CaseDetails),
            AbuseCaseDetails = abuseCaseDetailsMapper.Map(viewModel.AbuseCaseDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForUpdate),
        };
    }
}
