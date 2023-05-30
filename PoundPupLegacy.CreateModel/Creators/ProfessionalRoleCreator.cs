namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ProfessionalRoleCreatorFactory(
    IDatabaseInserterFactory<ProfessionalRoleToCreateForExistingPerson> professionalRoleInserterFactory
) : IEntityCreatorFactory<BasicProfessionalRole.ToCreateForExistingPerson>
{
    public async Task<IEntityCreator<BasicProfessionalRole.ToCreateForExistingPerson>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<BasicProfessionalRole.ToCreateForExistingPerson>( 
            new ()
            {
                await professionalRoleInserterFactory.CreateAsync(connection),
            }
        );
}

