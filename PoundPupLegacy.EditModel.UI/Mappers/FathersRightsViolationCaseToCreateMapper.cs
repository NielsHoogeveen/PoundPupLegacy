namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class FathersRightsViolationCaseToCreateMapper(
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<EditModel.LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<EditModel.CaseDetails, CreateModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<FathersRightsViolationCase.ToCreate, CreateModel.FathersRightsViolationCase.ToCreate>
{
    public CreateModel.FathersRightsViolationCase.ToCreate Map(FathersRightsViolationCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.FathersRightsViolationCase.ToCreate {
            Identification = new Identification.Possible { Id = null},
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
        };
    }
}
