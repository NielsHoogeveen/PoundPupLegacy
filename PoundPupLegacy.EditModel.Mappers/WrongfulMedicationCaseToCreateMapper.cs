namespace PoundPupLegacy.EditModel.Mappers;

internal class WrongfulMedicationCaseToCreateMapper(
    IMapper<NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper,
    IMapper<NameableDetails, CreateModel.NameableDetails.ForCreate> nameableMapper,
    IMapper<LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate> locatableMapper,
    IMapper<CaseDetails, CreateModel.CaseDetails.CaseDetailsForCreate> caseDetailMapper
) : IMapper<WrongfulMedicationCase.ToCreate, CreateModel.WrongfulMedicationCase.ToCreate>
{
    public CreateModel.WrongfulMedicationCase.ToCreate Map(WrongfulMedicationCase.ToCreate viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.WrongfulMedicationCase.ToCreate {
            Identification = new Identification.Possible { Id = null },
            NodeDetails = nodeDetailsMapper.Map(viewModel.NodeDetailsForCreate),
            NameableDetails = nameableMapper.Map(viewModel.NameableDetails),
            LocatableDetails = locatableMapper.Map(viewModel.LocatableDetailsToCreate),
            CaseDetails = caseDetailMapper.Map(viewModel.CaseDetails),
        };
    }
}
