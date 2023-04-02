﻿namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ProfessionalRoleCreator : IEntityCreator<ProfessionalRole>
{
    public async Task CreateAsync(IAsyncEnumerable<ProfessionalRole> professionalRoles, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var professionalRoleWriter = await ProfessionalRoleInserter.CreateAsync(connection);
        await using var memberOfCongressWriter = await MemberOfCongressInserter.CreateAsync(connection);
        await using var representativeWriter = await RepresentativeInserter.CreateAsync(connection);
        await using var senatorWriter = await SenatorInserter.CreateAsync(connection);

        await foreach (var professionalRole in professionalRoles) {
            await nodeWriter.InsertAsync(professionalRole);
            await searchableWriter.InsertAsync(professionalRole);
            await documentableWriter.InsertAsync(professionalRole);
            await professionalRoleWriter.InsertAsync(professionalRole);
            if (professionalRole is Representative representative) {
                await memberOfCongressWriter.InsertAsync(representative);
                await representativeWriter.InsertAsync(representative);
                foreach (var term in representative.HouseTerms) {
                    term.RepresentativeId = representative.Id;
                }
                await new HouseTermCreator().CreateAsync(representative.HouseTerms.ToAsyncEnumerable(), connection);
            }
            if (professionalRole is Senator senator) {
                await memberOfCongressWriter.InsertAsync(senator);
                await senatorWriter.InsertAsync(senator);
                foreach (var term in senator.SenateTerms) {
                    term.SenatorId = senator.Id;
                }
                await new SenateTermCreator().CreateAsync(senator.SenateTerms.ToAsyncEnumerable(), connection);
            }
        }
    }
}
