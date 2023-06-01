namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class CaseDetailsForCreateMapper : IMapper<EditModel.CaseDetails, CreateModel.CaseDetails.CaseDetailsForCreate>
{
    public CreateModel.CaseDetails.CaseDetailsForCreate Map(CaseDetails source)
    {
        return new CreateModel.CaseDetails.CaseDetailsForCreate {
            Date = source.Date,
            CaseCaseParties = new List<CreateModel.CaseCaseParties.ToCreate.ForNewCase>(),
        };
    }
}
