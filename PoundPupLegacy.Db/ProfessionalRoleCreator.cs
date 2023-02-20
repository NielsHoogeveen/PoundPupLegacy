namespace PoundPupLegacy.Db;

public class ProfessionalRoleCreator : IEntityCreator<ProfessionalRole>
{
    public static async Task CreateAsync(IAsyncEnumerable<ProfessionalRole> professionalRoles, NpgsqlConnection connection)
    {

        await using var professionalRoleWriter = await ProfessionalRoleWriter.CreateAsync(connection);
        await using var memberOfCongressWriter = await MemberOfCongressWriter.CreateAsync(connection);
        await using var representativeWriter = await RepresentativeWriter.CreateAsync(connection);
        await using var senatorWriter = await SenatorWriter.CreateAsync(connection);

        await foreach (var professionalRole in professionalRoles) 
        { 
            await professionalRoleWriter.WriteAsync(professionalRole);
            if(professionalRole is Representative representative)
            {
                await memberOfCongressWriter.WriteAsync(representative);
                await representativeWriter.WriteAsync(representative);
            }
            if (professionalRole is Senator senator)
            {
                await memberOfCongressWriter.WriteAsync(senator);
                await senatorWriter.WriteAsync(senator);
            }
        }
    }
}
