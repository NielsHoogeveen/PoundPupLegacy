using Npgsql;
using PoundPupLegacy.Db.Writers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class AttachmentTherapistCreator : IEntityCreator<AttachmentTherapist>
{
    public static void Create(IEnumerable<AttachmentTherapist> persons, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var locatableWriter = LocatableWriter.Create(connection);
        using var partyWriter = PartyWriter.Create(connection);
        using var personWriter = PersonWriter.Create(connection);
        using var attachmentTherapistWriter = AttachmentTherapistWriter.Create(connection);

        foreach (var person in persons)
        {
            nodeWriter.Write(person);
            documentableWriter.Write(person);
            locatableWriter.Write(person);
            partyWriter.Write(person);
            personWriter.Write(person);
            attachmentTherapistWriter.Write(person);
        }
    }
}
