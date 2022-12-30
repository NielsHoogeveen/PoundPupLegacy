using Npgsql;
using PoundPupLegacy.Db.Writers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class AdoptionExportRelationCreator : IEntityCreator<AdoptionExportRelation>
{
    public static void Create(IEnumerable<AdoptionExportRelation> adoptionExportRelations, NpgsqlConnection connection)
    {

        using var adoptionExportRelationWriter = AdoptionExportRelationWriter.Create(connection);

        foreach (var adoptionExportRelation in adoptionExportRelations)
        {
            adoptionExportRelationWriter.Write(adoptionExportRelation);
        }
    }
}
