namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class ProfessionalRoleCreatorFactory(
    IDatabaseInserterFactory<ProfessionalRoleToCreateForExistingPerson> professionalRoleInserterFactory,
    IDatabaseInserterFactory<MemberOfCongressToCreateForExistingPerson> memberOfCongressInserterFactory,
    IDatabaseInserterFactory<Representative.ToCreateForExistingPerson> representativeInserterFactory,
    IDatabaseInserterFactory<Senator.ToCreateForExistingPerson> senatorInserterFactory,
    IEntityCreatorFactory<SenateTerm.ToCreateForExistingSenator> senateTermCreatorFactory,
    IEntityCreatorFactory<HouseTerm.ToCreateForExistingRepresentative> houseTermCreatorFactory
) : IEntityCreatorFactory<ProfessionalRoleToCreateForExistingPerson>
{
    public async Task<IEntityCreator<ProfessionalRoleToCreateForExistingPerson>> CreateAsync(IDbConnection connection) =>
        new ProfessionalRoleCreator(
            new()
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
    List<IDatabaseInserter<ProfessionalRoleToCreateForExistingPerson>> inserters,
    IDatabaseInserter<MemberOfCongressToCreateForExistingPerson> memberOfCongressInserter,
    IDatabaseInserter<Representative.ToCreateForExistingPerson> representativeInserter,
    IDatabaseInserter<Senator.ToCreateForExistingPerson> senatorInserter,
    IEntityCreator<SenateTerm.ToCreateForExistingSenator> senateTermCreator,
    IEntityCreator<HouseTerm.ToCreateForExistingRepresentative> houseTermCreator

    ) : InsertingEntityCreator<ProfessionalRoleToCreateForExistingPerson>(inserters)
{
    public override async Task ProcessAsync(ProfessionalRoleToCreateForExistingPerson element)
    {
        await base.ProcessAsync(element);
        if (element is Representative.ToCreateForExistingPerson representative) {
            await memberOfCongressInserter.InsertAsync(representative);
            await representativeInserter.InsertAsync(representative);
            await houseTermCreator
                .CreateAsync(representative.RepresentativeDetailsToCreate.HouseTermToCreate
                .Select(x => x.Resolve(representative.Identification.Id!.Value))
                .ToAsyncEnumerable());
        }
        if (element is Senator.ToCreateForExistingPerson senator) {
            await memberOfCongressInserter.InsertAsync(senator);
            await senatorInserter.InsertAsync(senator);
            await senateTermCreator.CreateAsync(
                senator.SenatorDetailsToCreate.SenateTermToCreate
                .Select(x => x.ResolveSenator(senator.Identification.Id!.Value))
                .ToAsyncEnumerable());
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