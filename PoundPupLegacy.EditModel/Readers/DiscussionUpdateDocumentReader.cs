namespace PoundPupLegacy.EditModel.Readers;

internal sealed class DiscussionUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<ExistingDiscussion>
{
    protected override int NodeTypeId => Constants.DISCUSSION;
}

