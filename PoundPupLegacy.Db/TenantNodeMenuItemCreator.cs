﻿namespace PoundPupLegacy.Db;

public class TenantNodeMenuItemCreator: IEntityCreator<TenantNodeMenuItem>
{
    public static async Task CreateAsync(IAsyncEnumerable<TenantNodeMenuItem> tenantNodeMenuItems, NpgsqlConnection connection)
    {

        await using var menuItemWriter = await MenuItemWriter.CreateAsync(connection);
        await using var tenantNodeMenuItemWriter = await TenantNodeMenuItemWriter.CreateAsync(connection);
        
        await foreach(var tenantNodeMenuItem in tenantNodeMenuItems)
        {
            await menuItemWriter.WriteAsync(tenantNodeMenuItem);
            await tenantNodeMenuItemWriter.WriteAsync(tenantNodeMenuItem);
        }
        
    }
}
