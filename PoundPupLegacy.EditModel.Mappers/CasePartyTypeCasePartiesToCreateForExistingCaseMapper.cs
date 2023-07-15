﻿using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.EditModel.Mappers;

internal class CasePartyTypeCasePartiesToCreateForExistingCaseMapper : IEnumerableMapper<CasePartyTypeCaseParties, CaseCaseParties.ToCreate.ForExistingCase>
{
    public IEnumerable<CaseCaseParties.ToCreate.ForExistingCase> Map(IEnumerable<CasePartyTypeCaseParties> source)
    {
        foreach (var parties in source) {
            if (!parties.CaseId.HasValue)
                continue;
            yield return new CaseCaseParties.ToCreate.ForExistingCase {
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
