using PoundPupLegacy.DomainModel;
using PoundPupLegacy.DomainModel.Deleters;

namespace PoundPupLegacy.EditModel.Mappers;

internal class NodeDetailsForUpdateMapper(
    IEnumerableMapper<Tags.ToUpdate, ResolvedNodeTermToAdd> nodeTermsToAddMapper,
    IEnumerableMapper<Tags.ToUpdate, NodeTermToRemove> nodeTermsToDeleteMapper,
    IEnumerableMapper<TenantNode.ToCreateForExistingNode, DomainModel.TenantNode.ToCreate.ForExistingNode> tenantNodeToCreateMapper,
    IEnumerableMapper<TenantNode.ToUpdate, TenantNodeToDelete> tenantNodeDeleteMapper,
    IEnumerableMapper<TenantNode.ToUpdate, DomainModel.TenantNode.ToUpdate> tenantNodeUpdateMapper
    ) : IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate>
{
    public DomainModel.NodeDetails.ForUpdate Map(NodeDetails.ForUpdate source)
    {
        var now = DateTime.Now;
        return new DomainModel.NodeDetails.ForUpdate {
            AuthoringStatusId = 1,
            ChangedDateTime = now,
            Title = source.Title,
            TenantNodesToAdd = tenantNodeToCreateMapper.Map(source.TenantNodeDetailsForUpdate.TenantNodesToAdd).ToList(),
            TenantNodesToRemove = tenantNodeDeleteMapper.Map(source.TenantNodeDetailsForUpdate.TenantNodesToUpdate).ToList(),
            TenantNodesToUpdate = tenantNodeUpdateMapper.Map(source.TenantNodeDetailsForUpdate.TenantNodesToUpdate).ToList(),
            NodeTermsToAdd = nodeTermsToAddMapper.Map(source.TagsForUpdate).ToList(),
            NodeTermsToRemove = nodeTermsToDeleteMapper.Map(source.TagsForUpdate).ToList(),
            FilesToAdd = source.Files.Where(x => !x.HasBeenStored).Select(x => new DomainModel.File {
                Identification = new Identification.Possible { Id = null },
                Name = x.Name,
                Path = x.Path,
                MimeType = x.MimeType,
                Size = (int)x.Size,
                TenantFiles = source
                    .Tenants.Select(x => new TenantFile {
                        FileId = null,
                        TenantFileId = null,
                        TenantId = x.Id
                    }).ToList()
            }).ToList(),
            FileIdsToRemove = source.Files.Where(x => x.HasBeenDeleted).Select(x => x.Id!.Value).ToList(),
        };
    }
}
