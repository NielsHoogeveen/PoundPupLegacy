using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.CreateModel;

internal sealed class RepresentativeCreatorFactory(
    IDatabaseInserterFactory<ProfessionalRoleToCreateForExistingPerson> professionalRoleInserterFactory,
    IDatabaseInserterFactory<MemberOfCongressToCreateForExistingPerson> memberOfCongressInserterFactory,
    IDatabaseInserterFactory<Representative.RepresentativeToCreateForExistingPerson> representativeInserterFactory
) : IEntityCreatorFactory<Representative.RepresentativeToCreateForExistingPerson>
{
    public async Task<IEntityCreator<Representative.RepresentativeToCreateForExistingPerson>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<Representative.RepresentativeToCreateForExistingPerson>(
            new()
            {
                await professionalRoleInserterFactory.CreateAsync(connection),
                await memberOfCongressInserterFactory.CreateAsync(connection),
                await representativeInserterFactory.CreateAsync(connection)
            }
        );
}

