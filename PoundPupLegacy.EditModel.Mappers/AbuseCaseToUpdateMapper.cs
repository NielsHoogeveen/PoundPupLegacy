namespace PoundPupLegacy.EditModel.Mappers;

internal class AbuseCaseToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailsMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailsMapper,
    IMapper<LocatableDetails.ForUpdate, DomainModel.LocatableDetails.ForUpdate> locatableMapper,
    IMapper<CaseDetails, DomainModel.CaseDetails.CaseDetailsForUpdate> caseDetailsMapper,
    IMapper<AbuseCaseDetails, DomainModel.AbuseCaseDetails> abuseCaseDetailsMapper
    ) : IMapper<AbuseCase.ToUpdate, DomainModel.AbuseCase.ToUpdate>
{
    public DomainModel.AbuseCase.ToUpdate Map(AbuseCase.ToUpdate viewModel)
    {
        var now = DateTime.Now;
        return new DomainModel.AbuseCase.ToUpdate {
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
