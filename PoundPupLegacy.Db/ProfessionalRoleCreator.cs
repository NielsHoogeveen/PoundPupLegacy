namespace PoundPupLegacy.Db;

public class ProfessionalRoleCreator : IEntityCreator<ProfessionalRole>
{
    public static async Task CreateAsync(IAsyncEnumerable<ProfessionalRole> professionalRoles, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var professionalRoleWriter = await ProfessionalRoleWriter.CreateAsync(connection);
        await using var memberOfCongressWriter = await MemberOfCongressWriter.CreateAsync(connection);
        await using var representativeWriter = await RepresentativeWriter.CreateAsync(connection);
        await using var senatorWriter = await SenatorWriter.CreateAsync(connection);

        await foreach (var professionalRole in professionalRoles) {
            await nodeWriter.WriteAsync(professionalRole);
            await searchableWriter.WriteAsync(professionalRole);
            await documentableWriter.WriteAsync(professionalRole);
            await professionalRoleWriter.WriteAsync(professionalRole);
            if (professionalRole is Representative representative) {
                await memberOfCongressWriter.WriteAsync(representative);
                await representativeWriter.WriteAsync(representative);
                foreach (var term in representative.HouseTerms) {
                    term.RepresentativeId = representative.Id;
                }
                await HouseTermCreator.CreateAsync(representative.HouseTerms.ToAsyncEnumerable(), connection);
            }
            if (professionalRole is Senator senator) {
                await memberOfCongressWriter.WriteAsync(senator);
                await senatorWriter.WriteAsync(senator);
                foreach (var term in senator.SenateTerms) {
                    term.SenatorId = senator.Id;
                }
                await SenateTermCreator.CreateAsync(senator.SenateTerms.ToAsyncEnumerable(), connection);
            }
        }
    }
}
