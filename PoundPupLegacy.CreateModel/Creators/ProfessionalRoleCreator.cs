namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ProfessionalRoleCreator(
    IDatabaseInserterFactory<ProfessionalRole> professionalRoleInserterFactory,
    IDatabaseInserterFactory<MemberOfCongress> memberOfCongressInserterFactory,
    IDatabaseInserterFactory<Representative> representativeInserterFactory,
    IDatabaseInserterFactory<Senator> senatorInserterFactory,
    IEntityCreator<SenateTerm> senateTermCreator,
    IEntityCreator<HouseTerm> houseTermCreator
) : EntityCreator<ProfessionalRole>
{
    public override async Task CreateAsync(IAsyncEnumerable<ProfessionalRole> professionalRoles, IDbConnection connection)
    {
        await using var professionalRoleWriter = await professionalRoleInserterFactory.CreateAsync(connection);
        await using var memberOfCongressWriter = await memberOfCongressInserterFactory.CreateAsync(connection);
        await using var representativeWriter = await representativeInserterFactory.CreateAsync(connection);
        await using var senatorWriter = await senatorInserterFactory.CreateAsync(connection);

        await foreach (var professionalRole in professionalRoles) {
            await professionalRoleWriter.InsertAsync(professionalRole);
            if (professionalRole is Representative representative) {
                await memberOfCongressWriter.InsertAsync(representative);
                await representativeWriter.InsertAsync(representative);
                foreach (var term in representative.HouseTerms) {
                    term.RepresentativeId = representative.Id;
                }
                await houseTermCreator.CreateAsync(representative.HouseTerms.ToAsyncEnumerable(), connection);
            }
            if (professionalRole is Senator senator) {
                await memberOfCongressWriter.InsertAsync(senator);
                await senatorWriter.InsertAsync(senator);
                foreach (var term in senator.SenateTerms) {
                    term.SenatorId = senator.Id;
                }
                await senateTermCreator.CreateAsync(senator.SenateTerms.ToAsyncEnumerable(), connection);
            }
        }
    }
}
