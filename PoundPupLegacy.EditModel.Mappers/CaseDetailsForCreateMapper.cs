namespace PoundPupLegacy.EditModel.Mappers;

internal class CaseDetailsForCreateMapper : IMapper<CaseDetails, CreateModel.CaseDetails.CaseDetailsForCreate>
{
    public CreateModel.CaseDetails.CaseDetailsForCreate Map(CaseDetails source)
    {
        return new CreateModel.CaseDetails.CaseDetailsForCreate {
            Date = source.Date,
            CaseCaseParties = new List<CreateModel.CaseCaseParties.ToCreate.ForNewCase>(),
        };
    }
}
