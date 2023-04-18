namespace PoundPupLegacy.EditModel.Readers;

internal sealed class DiscussionUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<Discussion>
{
    protected override int NodeTypeId => Constants.DISCUSSION;
}

