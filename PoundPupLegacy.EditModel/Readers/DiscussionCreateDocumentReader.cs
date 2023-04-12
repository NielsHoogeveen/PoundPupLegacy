using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.EditModel.Readers;

public class DiscussionCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<DiscussionCreateDocumentReader>
{
}
public class DiscussionCreateDocumentReader : SimpleTextNodeCreateDocumentReader<Discussion>
{
    internal DiscussionCreateDocumentReader(NpgsqlCommand command) : base(command, Constants.DISCUSSION)
    {
    }
}


