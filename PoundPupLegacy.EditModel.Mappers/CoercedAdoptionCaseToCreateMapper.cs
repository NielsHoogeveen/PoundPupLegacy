namespace PoundPupLegacy.EditModel.Mappers;

internal class CoercedAdoptionCaseToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<LocatableDetails.ForCreate, DomainModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<CaseDetails, DomainModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<CoercedAdoptionCase.ToCreate, DomainModel.CoercedAdoptionCase.ToCreate>
{
    public DomainModel.CoercedAdoptionCase.ToCreate Map(CoercedAdoptionCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new DomainModel.CoercedAdoptionCase.ToCreate {
            Identification = new Identification.Possible { Id = null },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
        };
    }
}
