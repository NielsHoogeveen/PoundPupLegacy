using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class CasePartyTypeCasePartiesToUpdateMapper : IEnumerableMapper<CasePartyTypeCaseParties, CaseExistingCasePartiesToCreate>
{
    public IEnumerable<CaseExistingCasePartiesToCreate> Map(IEnumerable<CasePartyTypeCaseParties> source)
    {
        foreach(var parties in source) {
            if (!parties.Id.HasValue)
                continue;
            if (!parties.CaseId.HasValue)
                continue;
            yield return new CaseExistingCasePartiesToCreate {
                CaseId = parties.CaseId.Value,
                CasePartyTypeId = parties.CasePartyTypeId,
                CaseParties = new ExistingCaseParties {
                    Id = parties.Id.Value,
                    OrganizationIdsToAdd = parties.Organizations.Where(x => !x.HasBeenDeleted).Select(x => x.Organization.Id).ToList(),
                    PersonIdsToAdd = parties.Persons.Where(x => !x.HasBeenDeleted).Select(x => x.Person.Id).ToList(),
                    OrganizationIdsToRemove = parties.Organizations.Where(x => x.HasBeenDeleted).Select(x => x.Organization.Id).ToList(),
                    PersonIdsToRemove = parties.Persons.Where(x => x.HasBeenDeleted).Select(x => x.Person.Id).ToList(),
                    Organizations = parties.OrganizationsText,
                    Persons = parties.PersonsText,
                }
            };
        }
    }
}
