﻿namespace PoundPupLegacy.CreateModel.Creators;

public class AccessRolePrivilegeCreator : IEntityCreator<AccessRolePrivilege>
{
    public static async Task CreateAsync(IAsyncEnumerable<AccessRolePrivilege> accessRolePrivileges, NpgsqlConnection connection)
    {

        await using var accessRolePrivilegeWriter = await AccessRolePrivilegeInserter.CreateAsync(connection);

        await foreach (var accessRolePrivilege in accessRolePrivileges) {
            await accessRolePrivilegeWriter.InsertAsync(accessRolePrivilege);
        }

    }
}