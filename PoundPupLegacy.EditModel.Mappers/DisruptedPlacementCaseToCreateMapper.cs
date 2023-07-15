namespace PoundPupLegacy.EditModel.Mappers;

internal class DisruptedPlacementCaseToCreateMapper(
    IMapper<NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<NameableDetails, CreateModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<CaseDetails, CreateModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<DisruptedPlacementCase.ToCreate, CreateModel.DisruptedPlacementCase.ToCreate>
{
    public CreateModel.DisruptedPlacementCase.ToCreate Map(DisruptedPlacementCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.DisruptedPlacementCase.ToCreate {
            Identification = new Identification.Possible { Id = null },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
        };
    }
}
