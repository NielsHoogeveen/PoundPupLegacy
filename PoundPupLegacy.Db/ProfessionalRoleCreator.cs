namespace PoundPupLegacy.Db;

public class ProfessionalRoleCreator : IEntityCreator<ProfessionalRole>
{
    public static async Task CreateAsync(IAsyncEnumerable<ProfessionalRole> professionalRoles, NpgsqlConnection connection)
    {

        await using var professionalRoleWriter = await ProfessionalRoleWriter.CreateAsync(connection);
        await using var representativeWriter = await RepresentativeWriter.CreateAsync(connection);
        await using var senatorWriter = await SenatorWriter.CreateAsync(connection);

        await foreach (var professionalRole in professionalRoles) 
        { 
            await professionalRoleWriter.WriteAsync(professionalRole);
            if(professionalRoles is Representative representative)
            {
                await representativeWriter.WriteAsync(representative);
            }
            if (professionalRoles is Senator senator)
            {
                await senatorWriter.WriteAsync(senator);
            }
        }
    }
}
