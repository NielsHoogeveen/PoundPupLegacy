namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ProfessionalRoleCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableProfessionalRole> professionalRoleInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableMemberOfCongress> memberOfCongressInserterFactory,
    IDatabaseInserterFactory<Representative> representativeInserterFactory,
    IDatabaseInserterFactory<Senator> senatorInserterFactory,
    INodeCreatorFactory<EventuallyIdentifiableSenateTerm> senateTermCreatorFactory,
    INodeCreatorFactory<EventuallyIdentifiableHouseTerm> houseTermCreatorFactory
) : IInsertingEntityCreatorFactory<EventuallyIdentifiableProfessionalRole>
{
    public async Task<InsertingEntityCreator<EventuallyIdentifiableProfessionalRole>> CreateAsync(IDbConnection connection) =>
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
    List<IDatabaseInserter<EventuallyIdentifiableProfessionalRole>> inserters,
    IDatabaseInserter<EventuallyIdentifiableMemberOfCongress> memberOfCongressInserter,
    IDatabaseInserter<Representative> representativeInserter,
    IDatabaseInserter<Senator> senatorInserter,
    NodeCreator<EventuallyIdentifiableSenateTerm> senateTermCreator,
    NodeCreator<EventuallyIdentifiableHouseTerm> houseTermCreator

    ) : InsertingEntityCreator<EventuallyIdentifiableProfessionalRole>(inserters)
{
    public override async Task ProcessAsync(EventuallyIdentifiableProfessionalRole element)
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