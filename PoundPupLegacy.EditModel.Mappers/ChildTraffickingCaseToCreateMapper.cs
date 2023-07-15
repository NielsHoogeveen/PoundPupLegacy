namespace PoundPupLegacy.EditModel.Mappers;

internal class ChildTraffickingCaseToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<LocatableDetails.ForCreate, DomainModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<CaseDetails, DomainModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<ChildTraffickingCase.ToCreate, DomainModel.ChildTraffickingCase.ToCreate>
{
    public DomainModel.ChildTraffickingCase.ToCreate Map(ChildTraffickingCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new DomainModel.ChildTraffickingCase.ToCreate {
            Identification = new Identification.Possible { Id = null },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
            ChildTraffickingCaseDetails = new DomainModel.ChildTraffickingCaseDetails {
                NumberOfChildrenInvolved = viewModel.ResolvedChildTraffickingCaseDetails.NumberOfChildrenInvolved,
                CountryIdFrom = viewModel.ResolvedChildTraffickingCaseDetails.CountryFrom.Id,
            }
        };
    }
}
