using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class SenatorCreatorFactory(
    IDatabaseInserterFactory<ProfessionalRoleToCreateForExistingPerson> professionalRoleInserterFactory,
    IDatabaseInserterFactory<MemberOfCongressToCreateForExistingPerson> memberOfCongressInserterFactory,
    IDatabaseInserterFactory<Senator.ToCreateForExistingPerson> senatorInserterFactory,
    IEntityCreatorFactory<SenateTerm.ToCreateForExistingSenator> senateTermCreatorFactory
) : IEntityCreatorFactory<Senator.ToCreateForExistingPerson>
{
    public async Task<IEntityCreator<Senator.ToCreateForExistingPerson>> CreateAsync(IDbConnection connection) =>
        new SenatorCreator(
            new()
            {
                await professionalRoleInserterFactory.CreateAsync(connection),
                await memberOfCongressInserterFactory.CreateAsync(connection),
                await senatorInserterFactory.CreateAsync(connection)
            },
            await senateTermCreatorFactory.CreateAsync(connection)
        );
}

public sealed class SenatorCreator(
    List<IDatabaseInserter<Senator.ToCreateForExistingPerson>> inserters,
    IEntityCreator<SenateTerm.ToCreateForExistingSenator> senateTermCreator
    ) : InsertingEntityCreator<Senator.ToCreateForExistingPerson>(inserters)
{
    public override async Task ProcessAsync(Senator.ToCreateForExistingPerson element)
    {
        await base.ProcessAsync(element);
        await senateTermCreator
            .CreateAsync(element.SenatorDetailsToCreate.SenateTermToCreate
            .Select(x => x.ResolveSenator(element.Identification.Id!.Value))
            .ToAsyncEnumerable());

    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await senateTermCreator.DisposeAsync();
    }
}
