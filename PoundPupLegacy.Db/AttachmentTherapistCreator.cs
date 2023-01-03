﻿using PoundPupLegacy.Db.Readers;

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
        using var termWriter = TermWriter.Create(connection);
        using var termReader = TermReader.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);


        foreach (var person in persons)
        {
            nodeWriter.Write(person);
            documentableWriter.Write(person);
            locatableWriter.Write(person);
            partyWriter.Write(person);
            personWriter.Write(person);
            attachmentTherapistWriter.Write(person);
            EntityCreator.WriteTerms(person, termWriter, termReader, termHierarchyWriter);
        }
    }
}
