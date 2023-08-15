using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.EditModel.Mappers;

internal class NodeDetailsForCreateMapper(
    IEnumerableMapper<Tags.ToCreate, int> termIdsToAddMapper,
    IEnumerableMapper<TenantNode.ToCreateForNewNode, DomainModel.TenantNode.ToCreate.ForNewNode> tenantNodeMapper
    ) : IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate>
{
    public DomainModel.NodeDetails.ForCreate Map(NodeDetails.ForCreate source)
    {
        var now = DateTime.Now;
        var tenantNodesToAdd = source.Tenants.Select(x => x.TenantNode).Where(x => x is not null && !x.HasBeenStored && !x.HasBeenDeleted).OfType<TenantNode.ToCreateForNewNode>().ToList();
        return new DomainModel.NodeDetails.ForCreate {
            AuthoringStatusId = 1,
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = source.NodeTypeId,
            OwnerId = source.OwnerId,
            PublisherId = source.PublisherId,
            Title = source.Title,
            TermIds = termIdsToAddMapper.Map(source.TagsToCreate).ToList(),
            TenantNodes = tenantNodeMapper.Map(tenantNodesToAdd).ToList(),
            FilesToAdd = source.Files.Select(x => new DomainModel.File { 
                Identification = new Identification.Possible { Id = null },
                Name = x.Name,
                Path = x.Path,
                MimeType = x.MimeType,
                Size = (int)x.Size,
                TenantFiles = source.Tenants.Select(x => new TenantFile { 
                        FileId = null, 
                        TenantFileId = null, 
                        TenantId = x.Id
                    }).ToList()
            }).ToList()
        };
    }
}
