namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class AbusCaseCreateMapper(
    IMapper<EditModel.NodeDetails.NodeDetailsForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<EditModel.LocatableDetails.NewLocatableDetails, CreateModel.LocatableDetails.LocatableDetailsForCreate> locatableMapper,
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
            LocatableDetails = locatableMapper.Map(viewModel.NewLocatableDetails),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
            AbuseCaseDetails = abuseCaseDetailsMapper.Map(viewModel.AbuseCaseDetails),
        };
    }
}
