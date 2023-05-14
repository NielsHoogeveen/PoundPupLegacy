namespace PoundPupLegacy.EditModel.Readers;

internal sealed class DiscussionCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<NewDiscussion>
{
    protected override int NodeTypeId => Constants.DISCUSSION;
}
