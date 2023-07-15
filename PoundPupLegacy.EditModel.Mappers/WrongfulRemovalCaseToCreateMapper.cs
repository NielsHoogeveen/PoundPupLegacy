namespace PoundPupLegacy.EditModel.Mappers;

internal class WrongfulRemovalCaseToCreateMapper(
    IMapper<NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<NameableDetails, CreateModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<CaseDetails, CreateModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<WrongfulRemovalCase.ToCreate, CreateModel.WrongfulRemovalCase.ToCreate>
{
    public CreateModel.WrongfulRemovalCase.ToCreate Map(WrongfulRemovalCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.WrongfulRemovalCase.ToCreate {
            Identification = new Identification.Possible { Id = null },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsToCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
        };
    }
}
