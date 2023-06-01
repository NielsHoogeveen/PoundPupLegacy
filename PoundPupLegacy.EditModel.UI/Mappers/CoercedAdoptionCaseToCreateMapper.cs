namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class CoercedAdoptionCaseToCreateMapper(
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<EditModel.LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<EditModel.CaseDetails, CreateModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<CoercedAdoptionCase.ToCreate, CreateModel.CoercedAdoptionCase.ToCreate>
{
    public CreateModel.CoercedAdoptionCase.ToCreate Map(CoercedAdoptionCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.CoercedAdoptionCase.ToCreate {
            Identification = new Identification.Possible { Id = null},
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
        };
    }
}
