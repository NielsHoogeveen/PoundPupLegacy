﻿using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class CasePartyTypeCasePartiesToAddForExistingPartyMapper : IEnumerableMapper<CasePartyTypeCaseParties, ExistingCaseNewCaseParties>
{
    public IEnumerable<ExistingCaseNewCaseParties> Map(IEnumerable<CasePartyTypeCaseParties> source)
    {
        foreach(var parties in source) {
            if (!parties.CaseId.HasValue)
                continue;
            yield return new ExistingCaseNewCaseParties {
                CaseId = parties.CaseId.Value,
                CasePartyTypeId = parties.CasePartyTypeId,
                CaseParties = new NewCaseParties {
                    Id = null,
                    OrganizationIds = parties.Organizations.Where(x => !x.HasBeenDeleted).Select(x => x.Organization.Id).ToList(),
                    PersonIds = parties.Persons.Where(x => x.HasBeenDeleted).Select(x => x.Person.Id).ToList(),
                    Organizations = parties.OrganizationsText,
                    Persons = parties.PersonsText,
                }
            };
        }
    }
}
