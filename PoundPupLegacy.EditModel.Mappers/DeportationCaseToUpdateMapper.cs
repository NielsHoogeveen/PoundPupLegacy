namespace PoundPupLegacy.EditModel.Mappers;

internal class DeportationCaseToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailsMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailsMapper,
    IMapper<LocatableDetails.ForUpdate, DomainModel.LocatableDetails.ForUpdate> locatableMapper,
    IMapper<CaseDetails, DomainModel.CaseDetails.CaseDetailsForUpdate> caseDetailsMapper
    ) : IMapper<DeportationCase.ToUpdate, DomainModel.DeportationCase.ToUpdate>
{
    public DomainModel.DeportationCase.ToUpdate Map(DeportationCase.ToUpdate viewModel)
    {
        var now = DateTime.Now;
        return new DomainModel.DeportationCase.ToUpdate {
            Identification = new Identification.Certain {
                Id = viewModel.NodeIdentification.NodeId
            },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForUpdate),
            NameableDetails = nameableDetailsMapper.Map(viewModel.NameableDetails),
            CaseDetails = caseDetailsMapper.Map(viewModel.CaseDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForUpdate),
            DeportationCaseDetails = new DomainModel.DeportationCaseDetails {
                CountryIdTo = viewModel.DeportationCaseDetails.CountryTo?.Id,
                SubdivisionIdFrom = viewModel.DeportationCaseDetails.SubdivisionFrom?.Id,
            }
        };
    }
}
