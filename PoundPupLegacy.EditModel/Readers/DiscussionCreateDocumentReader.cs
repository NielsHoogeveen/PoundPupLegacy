namespace PoundPupLegacy.EditModel.Readers;

internal sealed class DiscussionCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<Discussion.NewDiscussion>
{
    protected override int NodeTypeId => Constants.DISCUSSION;
}
