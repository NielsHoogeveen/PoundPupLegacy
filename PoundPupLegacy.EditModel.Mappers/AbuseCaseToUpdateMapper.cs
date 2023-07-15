namespace PoundPupLegacy.EditModel.Mappers;

internal class AbuseCaseToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailsMapper,
    IMapper<NameableDetails, CreateModel.NameableDetails.ForUpdate> nameableDetailsMapper,
    IMapper<LocatableDetails.ForUpdate, CreateModel.LocatableDetails.ForUpdate> locatableMapper,
    IMapper<CaseDetails, CreateModel.CaseDetails.CaseDetailsForUpdate> caseDetailsMapper,
    IMapper<AbuseCaseDetails, CreateModel.AbuseCaseDetails> abuseCaseDetailsMapper
    ) : IMapper<AbuseCase.ToUpdate, CreateModel.AbuseCase.ToUpdate>
{
    public CreateModel.AbuseCase.ToUpdate Map(AbuseCase.ToUpdate viewModel)
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
