//using PoundPupLegacy.CreateModel;

//namespace PoundPupLegacy.EditModel.UI.Mappers;

//internal class CasePartyTypeCasePartiesToAddForNewPartyMapper : IEnumerableMapper<CasePartyTypeCaseParties, NewCaseNewCaseParties>
//{
//    public IEnumerable<NewCaseNewCaseParties> Map(IEnumerable<CasePartyTypeCaseParties> source)
//    {
//        foreach(var parties in source) {
//            yield return new NewCaseNewCaseParties {
//                CaseId = null,
//                CasePartyTypeId = parties.CasePartyTypeId,
//                CaseParties = new CasePartiesToCreate {
//                    Id = null,
//                    OrganizationIds = parties.Organizations.Where(x => !x.HasBeenDeleted).Select(x => x.Organization.Id).ToList(),
//                    PersonIds = parties.Persons.Where(x => x.HasBeenDeleted).Select(x => x.Person.Id).ToList(),
//                    Organizations = parties.OrganizationsText,
//                    Persons = parties.PersonsText,
//                }
//            };
//        }
//    }
//}
