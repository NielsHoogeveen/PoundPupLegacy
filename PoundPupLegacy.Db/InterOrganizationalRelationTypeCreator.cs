namespace PoundPupLegacy.Db;

public class InterOrganizationalRelationTypeCreator : IEntityCreator<InterOrganizationalRelationType>
{
    public static void Create(IEnumerable<InterOrganizationalRelationType> interOrganizationalRelationTypes, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var interOrganizationalRelationTypeWriter = InterOrganizationalRelationTypeWriter.Create(connection);

        foreach (var interOrganizationalRelationType in interOrganizationalRelationTypes)
        {
            nodeWriter.Write(interOrganizationalRelationType);
            nameableWriter.Write(interOrganizationalRelationType);
            interOrganizationalRelationTypeWriter.Write(interOrganizationalRelationType);
        }
    }
}
