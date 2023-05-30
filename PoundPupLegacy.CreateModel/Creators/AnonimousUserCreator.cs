﻿namespace PoundPupLegacy.CreateModel.Creators;

public interface IAnonimousUserCreator
{
    Task CreateAsync(IDbConnection connection);
}
internal sealed class AnonimousUserCreator(
    IDatabaseInserterFactory<Principal> principalInserterFactory,
    IDatabaseInserterFactory<Publisher> publisherInserterFactory
) : IAnonimousUserCreator
{
    public async Task CreateAsync(IDbConnection connection)
    {
        await using var principalWriter = await principalInserterFactory.CreateAsync(connection);
        await using var publisherWriter = await publisherInserterFactory.CreateAsync(connection);

        var user = new AnonymousUser {
            IdentificationForCreate = new Identification.IdentificationForCreate {
                Id = 0
            },
            Name = "anonymous",
        };
        await principalWriter.InsertAsync(user);
        await publisherWriter.InsertAsync(user);
    }
}
