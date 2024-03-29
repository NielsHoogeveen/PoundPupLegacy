﻿using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.EditModel.Mappers;

internal class OrganizationToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableDetailMapper,
    IEnumerableMapper<InterOrganizationalRelation.From.Complete.ToCreateForNewOrganization, DomainModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationFrom> interOrganizationalRelationFromMapper,
    IEnumerableMapper<InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization, DomainModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationTo> interOrganizationalRelationToMapper,
    IEnumerableMapper<OrganizationPoliticalEntityRelation.Complete.ToCreateForNewOrganization, PartyPoliticalEntityRelation.ToCreate.ForNewParty> partyPolitcalEntityCreateMapper,
    IEnumerableMapper<PersonOrganizationRelation.ForOrganization.Complete.ToCreateForNewOrganization, DomainModel.PersonOrganizationRelation.ToCreate.ForNewOrganization> personOrganizationRelationCreateMapper,
    IMapper<LocatableDetails.ForCreate, DomainModel.LocatableDetails.ForCreate> locatableDetailsMapper
) : IMapper<Organization.ToCreate, OrganizationToCreate>
{
    public OrganizationToCreate Map(Organization.ToCreate source)
    {
        return new BasicOrganization.ToCreate {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForCreate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
            LocatableDetails = locatableDetailsMapper.Map(source.LocatableDetailsForCreate),
            OrganizationDetails = new DomainModel.OrganizationDetails.ForCreate {
                EmailAddress = source.OrganizationDetails.EmailAddress,
                Established = source.OrganizationDetails.Establishment,
                Terminated = source.OrganizationDetails.Termination,
                WebsiteUrl = source.OrganizationDetails.WebSiteUrl,
                OrganizationTypeIdsToCreate = source.OrganizationDetails.OrganizationOrganizationTypes.Select(x => x.OrganizationTypeId).ToList(),
                PartyPoliticalEntityRelationsToCreate = partyPolitcalEntityCreateMapper.Map(source.OrganizationDetailsForCreate.OrganizationPoliticalEntityRelations.OfType<OrganizationPoliticalEntityRelation.Complete.ToCreateForNewOrganization>()).ToList(),
                PersonOrganizationRelationsToCreate = personOrganizationRelationCreateMapper.Map(source.OrganizationDetailsForCreate.PersonOrganizationRelations.OfType<PersonOrganizationRelation.ForOrganization.Complete.ToCreateForNewOrganization>()).ToList(),
                InterOrganizationalRelationsFrom = interOrganizationalRelationFromMapper.Map(source.OrganizationDetailsForCreate.InterOrganizationalRelationsFrom.OfType<InterOrganizationalRelation.From.Complete.ToCreateForNewOrganization>()).ToList(),
                InterOrganizationalRelationsTo = interOrganizationalRelationToMapper.Map(source.OrganizationDetailsForCreate.InterOrganizationalRelationsTo.OfType<InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization>()).ToList(),
            },
        };
    }
}
