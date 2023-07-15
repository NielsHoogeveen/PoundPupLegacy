namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class RepresentativeCreatorFactory(
    IDatabaseInserterFactory<ProfessionalRoleToCreateForExistingPerson> professionalRoleInserterFactory,
    IDatabaseInserterFactory<MemberOfCongressToCreateForExistingPerson> memberOfCongressInserterFactory,
    IDatabaseInserterFactory<Representative.ToCreateForExistingPerson> senatorInserterFactory,
    IEntityCreatorFactory<HouseTerm.ToCreateForExistingRepresentative> houseTermCreatorFactory

) : IEntityCreatorFactory<Representative.ToCreateForExistingPerson>
{
    public async Task<IEntityCreator<Representative.ToCreateForExistingPerson>> CreateAsync(IDbConnection connection) =>
        new RepresentativeCreator(
            new()
            {
                await professionalRoleInserterFactory.CreateAsync(connection),
                await memberOfCongressInserterFactory.CreateAsync(connection),
                await senatorInserterFactory.CreateAsync(connection)
            },
            await houseTermCreatorFactory.CreateAsync(connection)
        );
}

public sealed class RepresentativeCreator(
    List<IDatabaseInserter<Representative.ToCreateForExistingPerson>> inserters,
    IEntityCreator<HouseTerm.ToCreateForExistingRepresentative> houseTermCreator
    ) : InsertingEntityCreator<Representative.ToCreateForExistingPerson>(inserters)
{
    public override async Task ProcessAsync(Representative.ToCreateForExistingPerson element)
    {
        await base.ProcessAsync(element);
        await houseTermCreator
            .CreateAsync(element.RepresentativeDetailsToCreate.HouseTermToCreate
            .Select(x => x.Resolve(element.Identification.Id!.Value))
            .ToAsyncEnumerable());

    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await houseTermCreator.DisposeAsync();
    }
}
