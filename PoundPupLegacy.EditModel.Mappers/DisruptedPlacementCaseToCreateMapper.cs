namespace PoundPupLegacy.EditModel.Mappers;

internal class DisruptedPlacementCaseToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<LocatableDetails.ForCreate, DomainModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<CaseDetails, DomainModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<DisruptedPlacementCase.ToCreate, DomainModel.DisruptedPlacementCase.ToCreate>
{
    public DomainModel.DisruptedPlacementCase.ToCreate Map(DisruptedPlacementCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new DomainModel.DisruptedPlacementCase.ToCreate {
            Identification = new Identification.Possible { Id = null },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
        };
    }
}
