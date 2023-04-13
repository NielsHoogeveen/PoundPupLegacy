namespace PoundPupLegacy.EditModel.Readers;

using Reader = DiscussionCreateDocumentReader;

internal sealed class DiscussionCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<Discussion, Reader>
{
}
internal sealed class DiscussionCreateDocumentReader : SimpleTextNodeCreateDocumentReader<Discussion>
{
    public DiscussionCreateDocumentReader(NpgsqlCommand command) : base(command, Constants.DISCUSSION)
    {
    }
}


