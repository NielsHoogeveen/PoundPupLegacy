namespace PoundPupLegacy.EditModel.Mappers;

internal class CoercedAdoptionCaseToCreateMapper(
    IMapper<NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<NameableDetails, CreateModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<CaseDetails, CreateModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<CoercedAdoptionCase.ToCreate, CreateModel.CoercedAdoptionCase.ToCreate>
{
    public CreateModel.CoercedAdoptionCase.ToCreate Map(CoercedAdoptionCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.CoercedAdoptionCase.ToCreate {
            Identification = new Identification.Possible { Id = null },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
        };
    }
}
