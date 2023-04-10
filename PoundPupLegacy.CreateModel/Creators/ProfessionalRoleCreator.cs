namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ProfessionalRoleCreator : EntityCreator<ProfessionalRole>
{
    private readonly IDatabaseInserterFactory<ProfessionalRole> _professionalRoleInserterFactory;
    private readonly IDatabaseInserterFactory<MemberOfCongress> _memberOfCongressInserterFactory;
    private readonly IDatabaseInserterFactory<Representative> _representativeInserterFactory;
    private readonly IDatabaseInserterFactory<Senator> _senatorInserterFactory;
    private readonly IEntityCreator<SenateTerm> _senateTermCreator;
    private readonly IEntityCreator<HouseTerm> _houseTermCreator;
    public ProfessionalRoleCreator(
        IDatabaseInserterFactory<ProfessionalRole> professionalRoleInserterFactory, 
        IDatabaseInserterFactory<MemberOfCongress> memberOfCongressInserterFactory, 
        IDatabaseInserterFactory<Representative> representativeInserterFactory, 
        IDatabaseInserterFactory<Senator> senatorInserterFactory,
        IEntityCreator<SenateTerm> senateTermCreator,
        IEntityCreator<HouseTerm> houseTermCreator
    )
    {
        _professionalRoleInserterFactory = professionalRoleInserterFactory;
        _memberOfCongressInserterFactory = memberOfCongressInserterFactory;
        _representativeInserterFactory = representativeInserterFactory;
        _senatorInserterFactory = senatorInserterFactory;
        _senateTermCreator = senateTermCreator;
        _houseTermCreator = houseTermCreator;
    }
        
    public override async Task CreateAsync(IAsyncEnumerable<ProfessionalRole> professionalRoles, IDbConnection connection)
    {

        await using var professionalRoleWriter = await _professionalRoleInserterFactory.CreateAsync(connection);
        await using var memberOfCongressWriter = await _memberOfCongressInserterFactory.CreateAsync(connection);
        await using var representativeWriter = await _representativeInserterFactory.CreateAsync(connection);
        await using var senatorWriter = await _senatorInserterFactory.CreateAsync(connection);

        await foreach (var professionalRole in professionalRoles) {
            await professionalRoleWriter.InsertAsync(professionalRole);
            if (professionalRole is Representative representative) {
                await memberOfCongressWriter.InsertAsync(representative);
                await representativeWriter.InsertAsync(representative);
                foreach (var term in representative.HouseTerms) {
                    term.RepresentativeId = representative.Id;
                }
                await _houseTermCreator.CreateAsync(representative.HouseTerms.ToAsyncEnumerable(), connection);
            }
            if (professionalRole is Senator senator) {
                await memberOfCongressWriter.InsertAsync(senator);
                await senatorWriter.InsertAsync(senator);
                foreach (var term in senator.SenateTerms) {
                    term.SenatorId = senator.Id;
                }
                await _senateTermCreator.CreateAsync(senator.SenateTerms.ToAsyncEnumerable(), connection);
            }
        }
    }
}
