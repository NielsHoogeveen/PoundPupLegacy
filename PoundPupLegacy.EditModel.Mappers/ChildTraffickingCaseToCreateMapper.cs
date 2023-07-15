namespace PoundPupLegacy.EditModel.Mappers;

internal class ChildTraffickingCaseToCreateMapper(
    IMapper<NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<NameableDetails, CreateModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<CaseDetails, CreateModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<ChildTraffickingCase.ToCreate, CreateModel.ChildTraffickingCase.ToCreate>
{
    public CreateModel.ChildTraffickingCase.ToCreate Map(ChildTraffickingCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.ChildTraffickingCase.ToCreate {
            Identification = new Identification.Possible { Id = null },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
            ChildTraffickingCaseDetails = new CreateModel.ChildTraffickingCaseDetails {
                NumberOfChildrenInvolved = viewModel.ResolvedChildTraffickingCaseDetails.NumberOfChildrenInvolved,
                CountryIdFrom = viewModel.ResolvedChildTraffickingCaseDetails.CountryFrom.Id,
            }
        };
    }
}
