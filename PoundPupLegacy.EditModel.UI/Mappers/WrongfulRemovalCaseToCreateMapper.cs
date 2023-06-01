namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class WrongfulRemovalCaseToCreateMapper(
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<EditModel.LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<EditModel.CaseDetails, CreateModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<WrongfulRemovalCase.ToCreate, CreateModel.WrongfulRemovalCase.ToCreate>
{
    public CreateModel.WrongfulRemovalCase.ToCreate Map(WrongfulRemovalCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.WrongfulRemovalCase.ToCreate {
            Identification = new Identification.Possible { Id = null},
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsToCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
        };
    }
}
