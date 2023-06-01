namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class ChildTraffickingCaseToUpdateMapper(
    IMapper<EditModel.NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailsMapper,
    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForUpdate> nameableDetailsMapper,
    IMapper<EditModel.LocatableDetails.ForUpdate, CreateModel.LocatableDetails.ForUpdate> locatableMapper,
    IMapper<EditModel.CaseDetails, CreateModel.CaseDetails.CaseDetailsForUpdate> caseDetailsMapper
    ) : IMapper<EditModel.ChildTraffickingCase.ToUpdate, CreateModel.ChildTraffickingCase.ToUpdate>
{
    public CreateModel.ChildTraffickingCase.ToUpdate Map(EditModel.ChildTraffickingCase.ToUpdate viewModel)
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
