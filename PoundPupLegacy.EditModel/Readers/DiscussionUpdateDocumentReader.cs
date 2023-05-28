namespace PoundPupLegacy.EditModel.Readers;

internal sealed class DiscussionUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<Discussion.ExistingDiscussion>
{
    protected override int NodeTypeId => Constants.DISCUSSION;
}

