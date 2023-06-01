namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class CaseDetailsForUpdateMapper : IMapper<EditModel.CaseDetails, CreateModel.CaseDetails.CaseDetailsForUpdate>
{
    public CreateModel.CaseDetails.CaseDetailsForUpdate Map(CaseDetails source)
    {
        return new CreateModel.CaseDetails.CaseDetailsForUpdate {
            Date = source.Date,
            CasePartiesToAdd = new List<CreateModel.CaseCaseParties.ToCreate.ForExistingCase>(),
            CasePartiesToUpdate = new List<CreateModel.CaseCaseParties.ToUpdate>(),
        };
    }
}
