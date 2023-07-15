using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.EditModel.Mappers;

internal class CasePartyTypeCasePartiesToAddForNewPartyMapper : IEnumerableMapper<CasePartyTypeCaseParties, CaseCaseParties.ToCreate.ForNewCase>
{
    public IEnumerable<CaseCaseParties.ToCreate.ForNewCase> Map(IEnumerable<CasePartyTypeCaseParties> source)
    {
        foreach (var parties in source) {
            yield return new CaseCaseParties.ToCreate.ForNewCase {
                CasePartyTypeId = parties.CasePartyTypeId,
                CaseParties = new CaseParties.ToCreate {
                    Identification = new Identification.Possible {
                        Id = null,
                    },
                    OrganizationIds = parties.Organizations.Where(x => !x.HasBeenDeleted).Select(x => x.Organization.Id).ToList(),
                    PersonIds = parties.Persons.Where(x => x.HasBeenDeleted).Select(x => x.Person.Id).ToList(),
                    Organizations = parties.OrganizationsText,
                    Persons = parties.PersonsText,
                }
            };
        }
    }
}
