﻿namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PersonOrganizationRelationTypeCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiablePersonOrganizationRelationType> personOrganizationRelationTypeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiablePersonOrganizationRelationType>
{
    public async Task<IEntityCreator<EventuallyIdentifiablePersonOrganizationRelationType>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<EventuallyIdentifiablePersonOrganizationRelationType>(
            new ()
            {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await personOrganizationRelationTypeInserterFactory.CreateAsync(connection)
            }, 
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
