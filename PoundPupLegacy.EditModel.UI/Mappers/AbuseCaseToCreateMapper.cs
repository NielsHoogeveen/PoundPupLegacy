namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class AbuseCaseToCreateMapper(
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<EditModel.LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<EditModel.CaseDetails, CreateModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper,
    IMapper<EditModel.AbuseCaseDetails, CreateModel.AbuseCaseDetails> abuseCaseDetailsMapper
) : IMapper<AbuseCase.ToCreate, CreateModel.AbuseCase.ToCreate>
{
    public CreateModel.AbuseCase.ToCreate Map(AbuseCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.AbuseCase.ToCreate {
            Identification = new Identification.Possible { Id = null},
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
            AbuseCaseDetails = abuseCaseDetailsMapper.Map(viewModel.AbuseCaseDetails),
        };
    }
}
