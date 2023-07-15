namespace PoundPupLegacy.EditModel.Mappers;

internal class WrongfulRemovalCaseToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<LocatableDetails.ForCreate, DomainModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<CaseDetails, DomainModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<WrongfulRemovalCase.ToCreate, DomainModel.WrongfulRemovalCase.ToCreate>
{
    public DomainModel.WrongfulRemovalCase.ToCreate Map(WrongfulRemovalCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new DomainModel.WrongfulRemovalCase.ToCreate {
            Identification = new Identification.Possible { Id = null },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsToCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
        };
    }
}
