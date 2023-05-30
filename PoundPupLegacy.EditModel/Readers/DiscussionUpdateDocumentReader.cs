namespace PoundPupLegacy.EditModel.Readers;

internal sealed class DiscussionUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<Discussion.ToUpdate>
{
    protected override int NodeTypeId => Constants.DISCUSSION;
}

