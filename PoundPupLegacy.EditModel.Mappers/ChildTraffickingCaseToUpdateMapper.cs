namespace PoundPupLegacy.EditModel.Mappers;

internal class ChildTraffickingCaseToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailsMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailsMapper,
    IMapper<LocatableDetails.ForUpdate, DomainModel.LocatableDetails.ForUpdate> locatableMapper,
    IMapper<CaseDetails, DomainModel.CaseDetails.CaseDetailsForUpdate> caseDetailsMapper
    ) : IMapper<ChildTraffickingCase.ToUpdate, DomainModel.ChildTraffickingCase.ToUpdate>
{
    public DomainModel.ChildTraffickingCase.ToUpdate Map(ChildTraffickingCase.ToUpdate viewModel)
    {
        var now = DateTime.Now;
        return new DomainModel.ChildTraffickingCase.ToUpdate {
            Identification = new Identification.Certain {
                Id = viewModel.NodeIdentification.NodeId
            },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForUpdate),
            NameableDetails = nameableDetailsMapper.Map(viewModel.NameableDetails),
            CaseDetails = caseDetailsMapper.Map(viewModel.CaseDetails),
            ChildTraffickingCaseDetails = new DomainModel.ChildTraffickingCaseDetails {
                NumberOfChildrenInvolved = viewModel.ResolvedChildTraffickingCaseDetails.NumberOfChildrenInvolved,
                CountryIdFrom = viewModel.ResolvedChildTraffickingCaseDetails.CountryFrom.Id,
            },
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForUpdate),
        };
    }
}
