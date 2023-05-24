namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ProfessionalRoleCreatorFactory(
    IDatabaseInserterFactory<ProfessionalRole> professionalRoleInserterFactory,
    IDatabaseInserterFactory<MemberOfCongress> memberOfCongressInserterFactory,
    IDatabaseInserterFactory<Representative> representativeInserterFactory,
    IDatabaseInserterFactory<Senator> senatorInserterFactory,
    IEntityCreatorFactory<EventuallyIdentifiableSenateTerm> senateTermCreatorFactory,
    IEntityCreatorFactory<EventuallyIdentifiableHouseTerm> houseTermCreatorFactory
) : IInsertingEntityCreatorFactory<ProfessionalRole>
{
    public async Task<InsertingEntityCreator<ProfessionalRole>> CreateAsync(IDbConnection connection) =>
        new ProfessionalRoleCreator( 
            new ()
            {
                await professionalRoleInserterFactory.CreateAsync(connection),
            },
            await memberOfCongressInserterFactory.CreateAsync(connection),
            await representativeInserterFactory.CreateAsync(connection),
            await senatorInserterFactory.CreateAsync(connection),
            await senateTermCreatorFactory.CreateAsync(connection),
            await houseTermCreatorFactory.CreateAsync(connection)
        );
}

public class ProfessionalRoleCreator(
    List<IDatabaseInserter<ProfessionalRole>> inserters,
    IDatabaseInserter<MemberOfCongress> memberOfCongressInserter,
    IDatabaseInserter<Representative> representativeInserter,
    IDatabaseInserter<Senator> senatorInserter,
    IEntityCreator<EventuallyIdentifiableSenateTerm> senateTermCreator,
    IEntityCreator<EventuallyIdentifiableHouseTerm> houseTermCreator

    ) : InsertingEntityCreator<ProfessionalRole>(inserters)
{
    public override async Task ProcessAsync(ProfessionalRole element)
    {
        await base.ProcessAsync(element);
        if (element is Representative representative) {
            await memberOfCongressInserter.InsertAsync(representative);
            await representativeInserter.InsertAsync(representative);
            foreach (var term in representative.HouseTerms) {
                term.RepresentativeId = representative.Id;
            }
            await houseTermCreator.CreateAsync(representative.HouseTerms.ToAsyncEnumerable());
        }
        if (element is Senator senator) {
            await memberOfCongressInserter.InsertAsync(senator);
            await senatorInserter.InsertAsync(senator);
            foreach (var term in senator.SenateTerms) {
                term.SenatorId = senator.Id;
            }
            await senateTermCreator.CreateAsync(senator.SenateTerms.ToAsyncEnumerable());
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await memberOfCongressInserter.DisposeAsync();
        await representativeInserter.DisposeAsync();
        await senatorInserter.DisposeAsync();
        await houseTermCreator.DisposeAsync();
        await senateTermCreator.DisposeAsync();
        await houseTermCreator.DisposeAsync();
        await senateTermCreator.DisposeAsync();
    }
}