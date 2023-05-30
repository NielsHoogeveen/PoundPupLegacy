namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ProfessionalRoleCreatorFactory(
    IDatabaseInserterFactory<ProfessionalRoleToCreateForExistingPerson> professionalRoleInserterFactory
) : IEntityCreatorFactory<BasicProfessionalRole.BasicProfessionalRoleToCreateForExistingPerson>
{
    public async Task<IEntityCreator<BasicProfessionalRole.BasicProfessionalRoleToCreateForExistingPerson>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<BasicProfessionalRole.BasicProfessionalRoleToCreateForExistingPerson>( 
            new ()
            {
                await professionalRoleInserterFactory.CreateAsync(connection),
            }
        );
}

