using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.CreateModel;

internal sealed class RepresentativeCreatorFactory(
    IDatabaseInserterFactory<ProfessionalRoleToCreateForExistingPerson> professionalRoleInserterFactory,
    IDatabaseInserterFactory<MemberOfCongressToCreateForExistingPerson> memberOfCongressInserterFactory,
    IDatabaseInserterFactory<Representative.ToCreateForExistingPerson> representativeInserterFactory
) : IEntityCreatorFactory<Representative.ToCreateForExistingPerson>
{
    public async Task<IEntityCreator<Representative.ToCreateForExistingPerson>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<Representative.ToCreateForExistingPerson>(
            new()
            {
                await professionalRoleInserterFactory.CreateAsync(connection),
                await memberOfCongressInserterFactory.CreateAsync(connection),
                await representativeInserterFactory.CreateAsync(connection)
            }
        );
}

