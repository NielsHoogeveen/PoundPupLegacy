using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class CasePartyTypeCasePartiesToUpdateMapper : IEnumerableMapper<CasePartyTypeCaseParties, CaseCaseParties.ToUpdate>
{
    public IEnumerable<CaseCaseParties.ToUpdate> Map(IEnumerable<CasePartyTypeCaseParties> source)
    {
        foreach (var parties in source) {
            if (!parties.Id.HasValue)
                continue;
            if (!parties.CaseId.HasValue)
                continue;
            yield return new CaseCaseParties.ToUpdate {
                CaseId = parties.CaseId.Value,
                CasePartyTypeId = parties.CasePartyTypeId,
                CaseParties = new CaseParties.ToUpdate {
                    Identification = new Identification.Certain {
                        Id = parties.Id.Value,
                    },
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
