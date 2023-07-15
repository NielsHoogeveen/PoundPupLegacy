namespace PoundPupLegacy.EditModel.Mappers;

internal class DeportationCaseToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<LocatableDetails.ForCreate, DomainModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<CaseDetails, DomainModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<DeportationCase.ToCreate, DomainModel.DeportationCase.ToCreate>
{
    public DomainModel.DeportationCase.ToCreate Map(DeportationCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new DomainModel.DeportationCase.ToCreate {
            Identification = new Identification.Possible { Id = null },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
            DeportationCaseDetails = new DomainModel.DeportationCaseDetails {
                CountryIdTo = viewModel.DeportationCaseDetails.CountryTo?.Id,
                SubdivisionIdFrom = viewModel.DeportationCaseDetails.SubdivisionFrom?.Id,
            }
        };
    }
}
