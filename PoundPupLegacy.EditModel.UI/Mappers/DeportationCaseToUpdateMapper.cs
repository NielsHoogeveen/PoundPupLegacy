namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class DeportationCaseToUpdateMapper(
    IMapper<EditModel.NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailsMapper,
    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForUpdate> nameableDetailsMapper,
    IMapper<EditModel.LocatableDetails.ForUpdate, CreateModel.LocatableDetails.ForUpdate> locatableMapper,
    IMapper<EditModel.CaseDetails, CreateModel.CaseDetails.CaseDetailsForUpdate> caseDetailsMapper
    ) : IMapper<EditModel.DeportationCase.ToUpdate, CreateModel.DeportationCase.ToUpdate>
{
    public CreateModel.DeportationCase.ToUpdate Map(EditModel.DeportationCase.ToUpdate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.DeportationCase.ToUpdate {
            Identification = new Identification.Certain {
                Id = viewModel.NodeIdentification.NodeId
            },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForUpdate),
            NameableDetails = nameableDetailsMapper.Map(viewModel.NameableDetails),
            CaseDetails = caseDetailsMapper.Map(viewModel.CaseDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForUpdate),
            DeportationCaseDetails = new CreateModel.DeportationCaseDetails {
                CountryIdTo = viewModel.DeportationCaseDetails.CountryTo?.Id,
                SubdivisionIdFrom = viewModel.DeportationCaseDetails.SubdivisionFrom?.Id,
            }
        };
    }
}
