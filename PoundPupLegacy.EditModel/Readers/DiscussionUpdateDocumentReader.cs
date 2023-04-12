namespace PoundPupLegacy.EditModel.Readers;

using Reader = DiscussionUpdateDocumentReader;

public class DiscussionUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<Reader>
{
}

public class DiscussionUpdateDocumentReader : SimpleTextNodeUpdateDocumentReader<Discussion>
{
    internal DiscussionUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.DISCUSSION)
    {
    }
}