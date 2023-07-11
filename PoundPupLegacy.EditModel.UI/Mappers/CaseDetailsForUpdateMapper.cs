namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class CaseDetailsForUpdateMapper : IMapper<EditModel.CaseDetails, CreateModel.CaseDetails.CaseDetailsForUpdate>
{
    public CreateModel.CaseDetails.CaseDetailsForUpdate Map(CaseDetails source)
    {
        return new CreateModel.CaseDetails.CaseDetailsForUpdate {
            Date = source.Date,
            CasePartiesToAdd = source.CasePartyTypesCaseParties.Where(x => !x.Id.HasValue).Select(x => new CreateModel.CaseCaseParties.ToCreate.ForExistingCase {
               CaseId = x.CaseId!.Value,
               CasePartyTypeId = x.CasePartyTypeId,
               CaseParties = new CreateModel.CaseParties.ToCreate {
                   Identification = new Identification.Possible{ Id = null},
                   Organizations = x.OrganizationsText,
                   Persons = x.PersonsText,
                   OrganizationIds = x.Organizations.Where(x => !x.HasBeenStored).Select(x => x.Organization.Id).ToList(),
                   PersonIds = x.Persons.Where(x => !x.HasBeenStored).Select(x => x.Person.Id).ToList()               
               }
            }).ToList(),
            CasePartiesToUpdate = source.CasePartyTypesCaseParties.Where(x => x.Id.HasValue).Select(x => new CreateModel.CaseCaseParties.ToUpdate { 
                CaseId = x.CaseId!.Value,
                CasePartyTypeId = x.CasePartyTypeId,
                CaseParties = new CreateModel.CaseParties.ToUpdate { 
                    Identification = new Identification.Certain { Id = x.Id!.Value },
                    Organizations = x.OrganizationsText,
                    Persons = x.PersonsText,
                    OrganizationIdsToAdd = x.Organizations.Where(x => !x.HasBeenDeleted && !x.HasBeenStored).Select(x => x.Organization.Id).ToList(),
                    OrganizationIdsToRemove = x.Organizations.Where(x => x.HasBeenDeleted).Select(x => x.Organization.Id).ToList().ToList(),
                    PersonIdsToAdd = x.Persons.Where(x => !x.HasBeenDeleted && !x.HasBeenStored).Select(x => x.Person.Id).ToList(),
                    PersonIdsToRemove = x.Persons.Where(x => x.HasBeenDeleted).Select(x => x.Person.Id).ToList().ToList(),
                }
            }).ToList(),
        };
    }
}
