namespace PoundPupLegacy.EditModel.Mappers;

internal class FathersRightsViolationCaseToCreateMapper(
    IMapper<NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<NameableDetails, CreateModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<CaseDetails, CreateModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<FathersRightsViolationCase.ToCreate, CreateModel.FathersRightsViolationCase.ToCreate>
{
    public CreateModel.FathersRightsViolationCase.ToCreate Map(FathersRightsViolationCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.FathersRightsViolationCase.ToCreate {
            Identification = new Identification.Possible { Id = null },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
        };
    }
}
