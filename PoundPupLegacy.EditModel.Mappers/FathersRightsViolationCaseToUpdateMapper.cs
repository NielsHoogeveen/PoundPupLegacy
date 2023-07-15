namespace PoundPupLegacy.EditModel.Mappers;

internal class FathersRightsViolationCaseToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailsMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailsMapper,
    IMapper<LocatableDetails.ForUpdate, DomainModel.LocatableDetails.ForUpdate> locatableMapper,
    IMapper<CaseDetails, DomainModel.CaseDetails.CaseDetailsForUpdate> caseDetailsMapper
    ) : IMapper<FathersRightsViolationCase.ToUpdate, DomainModel.FathersRightsViolationCase.ToUpdate>
{
    public DomainModel.FathersRightsViolationCase.ToUpdate Map(FathersRightsViolationCase.ToUpdate viewModel)
    {
        var now = DateTime.Now;
        return new DomainModel.FathersRightsViolationCase.ToUpdate {
            Identification = new Identification.Certain {
                Id = viewModel.NodeIdentification.NodeId
            },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForUpdate),
            NameableDetails = nameableDetailsMapper.Map(viewModel.NameableDetails),
            CaseDetails = caseDetailsMapper.Map(viewModel.CaseDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForUpdate),
        };
    }
}
