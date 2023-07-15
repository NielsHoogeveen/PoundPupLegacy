﻿namespace PoundPupLegacy.EditModel.Mappers;

internal class NodeDetailsForCreateMapper(
    IEnumerableMapper<Tags.ToCreate, int> termIdsToAddMapper,
    IEnumerableMapper<TenantNode.ToCreateForNewNode, DomainModel.TenantNode.ToCreate.ForNewNode> tenantNodeMapper
    ) : IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate>
{
    public DomainModel.NodeDetails.ForCreate Map(NodeDetails.ForCreate source)
    {
        var now = DateTime.Now;
        return new DomainModel.NodeDetails.ForCreate {
            AuthoringStatusId = 1,
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = source.NodeTypeId,
            OwnerId = source.OwnerId,
            PublisherId = source.PublisherId,
            Title = source.Title,
            TermIds = termIdsToAddMapper.Map(source.TagsToCreate).ToList(),
            TenantNodes = tenantNodeMapper.Map(source.TenantNodeDetailsForCreate.TenantNodesToAdd).ToList(),
        };
    }
}
