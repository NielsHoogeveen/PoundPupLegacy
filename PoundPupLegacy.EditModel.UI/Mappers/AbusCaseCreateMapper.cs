namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class AbusCaseCreateMapper(
    IMapper<EditModel.NodeDetails.NodeDetailsForCreate, CreateModel.NodeDetails.NodeDetailsForCreate> nodeDetailsMapper,
    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.NameableDetailsForCreate> nameableMapper,
    IMapper<EditModel.LocatableDetails.NewLocatableDetails, CreateModel.LocatableDetails.LocatableDetailsForCreate> locatableMapper,
    IMapper<EditModel.CaseDetails, CreateModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper,
    IMapper<EditModel.AbuseCaseDetails, CreateModel.AbuseCaseDetails> abuseCaseDetailsMapper
) : IMapper<AbuseCase.NewAbuseCase, CreateModel.AbuseCase.ToCreate>
{
    public CreateModel.AbuseCase.ToCreate Map(AbuseCase.NewAbuseCase viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.AbuseCase.ToCreate {
            IdentificationForCreate = new Identification.Possible { Id = null},
            NodeDetailsForCreate = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetailsForCreate = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetailsForCreate = locatableMapper.Map(viewModel.NewLocatableDetails),
            CaseDetailsForCreate = caseDetailMapper.Map(viewModel.CaseDetails),
            AbuseCaseDetails = abuseCaseDetailsMapper.Map(viewModel.AbuseCaseDetails),
        };
    }
}
