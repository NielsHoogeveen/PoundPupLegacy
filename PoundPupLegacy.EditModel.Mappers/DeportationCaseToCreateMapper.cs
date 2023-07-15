namespace PoundPupLegacy.EditModel.Mappers;

internal class DeportationCaseToCreateMapper(
    IMapper<NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<NameableDetails, CreateModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<CaseDetails, CreateModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<DeportationCase.ToCreate, CreateModel.DeportationCase.ToCreate>
{
    public CreateModel.DeportationCase.ToCreate Map(DeportationCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.DeportationCase.ToCreate {
            Identification = new Identification.Possible { Id = null },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
            DeportationCaseDetails = new CreateModel.DeportationCaseDetails {
                CountryIdTo = viewModel.DeportationCaseDetails.CountryTo?.Id,
                SubdivisionIdFrom = viewModel.DeportationCaseDetails.SubdivisionFrom?.Id,
            }
        };
    }
}
