namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class DisruptedPlacementCaseToCreateMapper(
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<EditModel.LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<EditModel.CaseDetails, CreateModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<DisruptedPlacementCase.ToCreate, CreateModel.DisruptedPlacementCase.ToCreate>
{
    public CreateModel.DisruptedPlacementCase.ToCreate Map(DisruptedPlacementCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.DisruptedPlacementCase.ToCreate {
            Identification = new Identification.Possible { Id = null},
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
        };
    }
}
