using PoundPupLegacy.Common;

namespace PoundPupLegacy.Services.Implementation;

public class SubgroupNodeTypeService(ISiteDataService siteDataService) : ISubgroupNodeTypeService
{
    public async Task<List<CreateableNodeTypes>> GetCreateableNodeTypes(int subgroupId, int userId)
    {
        var user = await siteDataService.GetUser(userId);
        return user!.CreateActions.Where(x => x.UserGroupId == subgroupId).Select(x => new CreateableNodeTypes {
            Id = x.NodeTypeId,
            Name = x.NodeTypeName
        }).ToList();
    }
}
