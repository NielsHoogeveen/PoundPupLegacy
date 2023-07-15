using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.EditModel.Mappers;

internal class CaseDetailsForCreateMapper : IMapper<CaseDetails, DomainModel.CaseDetails.CaseDetailsForCreate>
{
    public DomainModel.CaseDetails.CaseDetailsForCreate Map(CaseDetails source)
    {
        return new DomainModel.CaseDetails.CaseDetailsForCreate {
            Date = source.Date,
            CaseCaseParties = new List<CaseCaseParties.ToCreate.ForNewCase>(),
        };
    }
}
