namespace PoundPupLegacy.EditModel.Mappers;

internal class AbuseCaseToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<LocatableDetails.ForCreate, DomainModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<CaseDetails, DomainModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper,
    IMapper<AbuseCaseDetails, DomainModel.AbuseCaseDetails> abuseCaseDetailsMapper
) : IMapper<AbuseCase.ToCreate, DomainModel.AbuseCase.ToCreate>
{
    public DomainModel.AbuseCase.ToCreate Map(AbuseCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new DomainModel.AbuseCase.ToCreate {
            Identification = new Identification.Possible { Id = null },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
            AbuseCaseDetails = abuseCaseDetailsMapper.Map(viewModel.AbuseCaseDetails),
        };
    }
}
