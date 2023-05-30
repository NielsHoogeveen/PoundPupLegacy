namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SenatorCreatorFactory(
    IDatabaseInserterFactory<ProfessionalRoleToCreateForExistingPerson> professionalRoleInserterFactory,
    IDatabaseInserterFactory<MemberOfCongressToCreateForExistingPerson> memberOfCongressInserterFactory,
    IDatabaseInserterFactory<Senator.SenatorToCreateForExistingPerson> senatorInserterFactory
) : IEntityCreatorFactory<Senator.SenatorToCreateForExistingPerson>
{
    public async Task<IEntityCreator<Senator.SenatorToCreateForExistingPerson>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<Senator.SenatorToCreateForExistingPerson>( 
            new ()
            {
                await professionalRoleInserterFactory.CreateAsync(connection),
                await memberOfCongressInserterFactory.CreateAsync(connection),
                await senatorInserterFactory.CreateAsync(connection)
            }
        );
}

