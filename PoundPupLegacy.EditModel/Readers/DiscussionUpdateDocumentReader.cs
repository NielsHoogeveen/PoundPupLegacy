using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.EditModel.Readers;

public class DiscussionUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<DiscussionUpdateDocumentReader>
{
}

public class DiscussionUpdateDocumentReader : SimpleTextNodeUpdateDocumentReader<Discussion>
{
    internal DiscussionUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.DISCUSSION)
    {
    }
}