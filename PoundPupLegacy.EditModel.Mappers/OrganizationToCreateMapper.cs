using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.Mappers;

internal class OrganizationToCreateMapper(
    IMapper<NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<NameableDetails, CreateModel.NameableDetails.ForCreate> nameableDetailMapper,
    IEnumerableMapper<InterOrganizationalRelation.From.Complete.ToCreateForNewOrganization, CreateModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationFrom> interOrganizationalRelationFromMapper,
    IEnumerableMapper<InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization, CreateModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationTo> interOrganizationalRelationToMapper,
    IEnumerableMapper<OrganizationPoliticalEntityRelation.Complete.ToCreateForNewOrganization, PartyPoliticalEntityRelation.ToCreate.ForNewParty> partyPolitcalEntityCreateMapper,
    IEnumerableMapper<PersonOrganizationRelation.ForOrganization.Complete.ToCreateForNewOrganization, CreateModel.PersonOrganizationRelation.ToCreate.ForNewOrganization> personOrganizationRelationCreateMapper,
    IMapper<LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate> locatableDetailsMapper
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
            OrganizationDetails = new CreateModel.OrganizationDetails.ForCreate {
                EmailAddress = source.OrganizationDetails.EmailAddress,
                Established = source.OrganizationDetails.Establishment,
                Terminated = source.OrganizationDetails.Termination,
                WebsiteUrl = source.OrganizationDetails.WebSiteUrl,
                OrganizationTypeIds = source.OrganizationDetails.OrganizationTypes.Select(x => x.Id).ToList(),
                PartyPoliticalEntityRelationsToCreate = partyPolitcalEntityCreateMapper.Map(source.OrganizationDetailsForCreate.OrganizationPoliticalEntityRelationsToCreate).ToList(),
                PersonOrganizationRelationsToCreate = personOrganizationRelationCreateMapper.Map(source.OrganizationDetailsForCreate.PersonOrganizationRelationsToCreate).ToList(),
                InterOrganizationalRelationsFrom = interOrganizationalRelationFromMapper.Map(source.OrganizationDetailsForCreate.InterOrganizationalRelationsFromToCreate).ToList(),
                InterOrganizationalRelationsTo = interOrganizationalRelationToMapper.Map(source.OrganizationDetailsForCreate.InterOrganizationalRelationsToToCreate).ToList(),
            },
        };
    }
}
