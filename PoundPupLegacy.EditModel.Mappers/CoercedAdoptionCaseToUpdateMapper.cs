namespace PoundPupLegacy.EditModel.Mappers;

internal class CoercedAdoptionCaseToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailsMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailsMapper,
    IMapper<LocatableDetails.ForUpdate, DomainModel.LocatableDetails.ForUpdate> locatableMapper,
    IMapper<CaseDetails, DomainModel.CaseDetails.CaseDetailsForUpdate> caseDetailsMapper
    ) : IMapper<CoercedAdoptionCase.ToUpdate, DomainModel.CoercedAdoptionCase.ToUpdate>
{
    public DomainModel.CoercedAdoptionCase.ToUpdate Map(CoercedAdoptionCase.ToUpdate viewModel)
    {
        var now = DateTime.Now;
        return new DomainModel.CoercedAdoptionCase.ToUpdate {
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
