using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class OrganizationToCreateMapper(
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForCreate> nameableDetailMapper,
    IEnumerableMapper<EditModel.InterOrganizationalRelation.From.Complete.ToCreateForNewOrganization, CreateModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationFrom> interOrganizationalRelationFromMapper,
    IEnumerableMapper<EditModel.InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization, CreateModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationTo> interOrganizationalRelationToMapper,
    IEnumerableMapper<EditModel.OrganizationPoliticalEntityRelation.Complete.ToCreateForNewOrganization, PartyPoliticalEntityRelation.ToCreate.ForNewParty> partyPolitcalEntityCreateMapper,
    IEnumerableMapper<EditModel.PersonOrganizationRelation.ForOrganization.Complete.ToCreateForNewOrganization, CreateModel.PersonOrganizationRelation.ToCreate.ForNewOrganization> personOrganizationRelationCreateMapper,
    IMapper<EditModel.LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate> locatableDetailsMapper
) : IMapper<Organization.ToCreate, CreateModel.OrganizationToCreate>
{
    public CreateModel.OrganizationToCreate Map(Organization.ToCreate source)
    {
        return new CreateModel.BasicOrganization.ToCreate {
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
