using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class CasePartyTypeCasePartiesToAddForExistingPartyMapper : IEnumerableMapper<CasePartyTypeCaseParties, CreateModel.CaseCaseParties.ToCreate.ForExistingCase>
{
    public IEnumerable<CreateModel.CaseCaseParties.ToCreate.ForExistingCase> Map(IEnumerable<CasePartyTypeCaseParties> source)
    {
        foreach (var parties in source) {
            if (!parties.CaseId.HasValue)
                continue;
            yield return new CreateModel.CaseCaseParties.ToCreate.ForExistingCase {
                CaseId = parties.CaseId.Value,
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
