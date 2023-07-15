namespace PoundPupLegacy.EditModel.Mappers;

internal class ChildTraffickingCaseToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailsMapper,
    IMapper<NameableDetails, CreateModel.NameableDetails.ForUpdate> nameableDetailsMapper,
    IMapper<LocatableDetails.ForUpdate, CreateModel.LocatableDetails.ForUpdate> locatableMapper,
    IMapper<CaseDetails, CreateModel.CaseDetails.CaseDetailsForUpdate> caseDetailsMapper
    ) : IMapper<ChildTraffickingCase.ToUpdate, CreateModel.ChildTraffickingCase.ToUpdate>
{
    public CreateModel.ChildTraffickingCase.ToUpdate Map(ChildTraffickingCase.ToUpdate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.ChildTraffickingCase.ToUpdate {
            Identification = new Identification.Certain {
                Id = viewModel.NodeIdentification.NodeId
            },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForUpdate),
            NameableDetails = nameableDetailsMapper.Map(viewModel.NameableDetails),
            CaseDetails = caseDetailsMapper.Map(viewModel.CaseDetails),
            ChildTraffickingCaseDetails = new CreateModel.ChildTraffickingCaseDetails {
                NumberOfChildrenInvolved = viewModel.ResolvedChildTraffickingCaseDetails.NumberOfChildrenInvolved,
                CountryIdFrom = viewModel.ResolvedChildTraffickingCaseDetails.CountryFrom.Id,
            },
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForUpdate),
        };
    }
}
