namespace PoundPupLegacy.Db;

public class AdoptionExportRelationCreator : IEntityCreator<AdoptionExportRelation>
{
    public static async Task CreateAsync(IEnumerable<AdoptionExportRelation> adoptionExportRelations, NpgsqlConnection connection)
    {

        await using var adoptionExportRelationWriter = await AdoptionExportRelationWriter.CreateAsync(connection);

        foreach (var adoptionExportRelation in adoptionExportRelations)
        {
            await adoptionExportRelationWriter.WriteAsync(adoptionExportRelation);
        }
    }
}
