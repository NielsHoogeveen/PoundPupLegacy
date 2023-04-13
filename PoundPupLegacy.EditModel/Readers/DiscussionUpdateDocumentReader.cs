namespace PoundPupLegacy.EditModel.Readers;

using Reader = DiscussionUpdateDocumentReader;

internal sealed class DiscussionUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<Discussion, Reader>
{
}

internal sealed class DiscussionUpdateDocumentReader : SimpleTextNodeUpdateDocumentReader<Discussion>
{
    public DiscussionUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.DISCUSSION)
    {
    }
}