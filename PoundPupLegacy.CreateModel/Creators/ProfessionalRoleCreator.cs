namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ProfessionalRoleCreatorFactory(
    IDatabaseInserterFactory<ResolvedProfessionalRoleToCreate> professionalRoleInserterFactory,
    IDatabaseInserterFactory<MemberOfCongressToCreate> memberOfCongressInserterFactory,
    IDatabaseInserterFactory<Representative.RepresentativeToCreate> representativeInserterFactory,
    IDatabaseInserterFactory<Senator.SenatorToCreate> senatorInserterFactory,
    IEntityCreatorFactory<SenateTerm.SenateTermToCreate> senateTermCreatorFactory,
    IEntityCreatorFactory<HouseTerm.HouseTermToCreate> houseTermCreatorFactory
) : IEntityCreatorFactory<ResolvedProfessionalRoleToCreate>
{
    public async Task<IEntityCreator<ResolvedProfessionalRoleToCreate>> CreateAsync(IDbConnection connection) =>
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
    List<IDatabaseInserter<ResolvedProfessionalRoleToCreate>> inserters,
    IDatabaseInserter<MemberOfCongressToCreate> memberOfCongressInserter,
    IDatabaseInserter<Representative.RepresentativeToCreate> representativeInserter,
    IDatabaseInserter<Senator.SenatorToCreate> senatorInserter,
    IEntityCreator<SenateTerm.SenateTermToCreate> senateTermCreator,
    IEntityCreator<HouseTerm.HouseTermToCreate> houseTermCreator

    ) : InsertingEntityCreator<ResolvedProfessionalRoleToCreate>(inserters)
{
    public override async Task ProcessAsync(ResolvedProfessionalRoleToCreate element)
    {
        await base.ProcessAsync(element);
        if (element is NewRepresentativeAsExistingPerson representative) {
            await memberOfCongressInserter.InsertAsync(representative);
            await representativeInserter.InsertAsync(representative);
            foreach (var term in representative.HouseTerms) {
                term.RepresentativeId = representative.Id;
            }
            await houseTermCreator.CreateAsync(representative.HouseTerms.ToAsyncEnumerable());
        }
        if (element is NewSenatorAsExistingPerson senator) {
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