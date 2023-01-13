﻿namespace PoundPupLegacy.Db.Writers;

internal sealed class UserRoleWriter : IDatabaseWriter<UserRole>
{
    public static async Task<DatabaseWriter<UserRole>> CreateAsync(NpgsqlConnection connection)
    {
        return new SingleIdWriter<UserRole>(await SingleIdWriter.CreateSingleIdCommandAsync("user_role", connection));
    }
}
