namespace PoundPupLegacy.EditModel.Mappers;

internal class WrongfulMedicationCaseToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<LocatableDetails.ForCreate, DomainModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<CaseDetails, DomainModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<WrongfulMedicationCase.ToCreate, DomainModel.WrongfulMedicationCase.ToCreate>
{
    public DomainModel.WrongfulMedicationCase.ToCreate Map(WrongfulMedicationCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new DomainModel.WrongfulMedicationCase.ToCreate {
            Identification = new Identification.Possible { Id = null },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsForCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
        };
    }
}
