namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SenatorCreatorFactory(
    IDatabaseInserterFactory<ProfessionalRoleToCreateForExistingPerson> professionalRoleInserterFactory,
    IDatabaseInserterFactory<MemberOfCongressToCreateForExistingPerson> memberOfCongressInserterFactory,
    IDatabaseInserterFactory<Senator.ToCreateForExistingPerson> senatorInserterFactory
) : IEntityCreatorFactory<Senator.ToCreateForExistingPerson>
{
    public async Task<IEntityCreator<Senator.ToCreateForExistingPerson>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<Senator.ToCreateForExistingPerson>( 
            new ()
            {
                await professionalRoleInserterFactory.CreateAsync(connection),
                await memberOfCongressInserterFactory.CreateAsync(connection),
                await senatorInserterFactory.CreateAsync(connection)
            }
        );
}

