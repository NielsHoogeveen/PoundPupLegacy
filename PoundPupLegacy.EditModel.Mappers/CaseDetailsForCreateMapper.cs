using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.EditModel.Mappers;

internal class CaseDetailsForCreateMapper : IMapper<CaseDetails, DomainModel.CaseDetails.CaseDetailsForCreate>
{
    public DomainModel.CaseDetails.CaseDetailsForCreate Map(CaseDetails source)
    {
        return new DomainModel.CaseDetails.CaseDetailsForCreate {
            Date = source.Date,
            CaseCaseParties = source.CasePartyTypesCaseParties.Where(x => !x.Id.HasValue && (x.Persons.Any() || x.Organizations.Any() || !string.IsNullOrEmpty(x.OrganizationsText) || !string.IsNullOrEmpty(x.PersonsText))).Select(x => new CaseCaseParties.ToCreate.ForNewCase {
                CasePartyTypeId = x.CasePartyTypeId,
                CaseParties = new CaseParties.ToCreate {
                    Identification = new Identification.Possible { Id = null },
                    Organizations = x.OrganizationsText,
                    Persons = x.PersonsText,
                    OrganizationIds = x.Organizations.Where(x => !x.HasBeenStored).Select(x => x.Organization.Id).ToList(),
                    PersonIds = x.Persons.Where(x => !x.HasBeenStored).Select(x => x.Person.Id).ToList()
                }
            }).ToList(),
        };
    }
}
