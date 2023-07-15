namespace PoundPupLegacy.EditModel.Mappers;

internal class CoercedAdoptionCaseToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailsMapper,
    IMapper<NameableDetails, CreateModel.NameableDetails.ForUpdate> nameableDetailsMapper,
    IMapper<LocatableDetails.ForUpdate, CreateModel.LocatableDetails.ForUpdate> locatableMapper,
    IMapper<CaseDetails, CreateModel.CaseDetails.CaseDetailsForUpdate> caseDetailsMapper
    ) : IMapper<CoercedAdoptionCase.ToUpdate, CreateModel.CoercedAdoptionCase.ToUpdate>
{
    public CreateModel.CoercedAdoptionCase.ToUpdate Map(CoercedAdoptionCase.ToUpdate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.CoercedAdoptionCase.ToUpdate {
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
