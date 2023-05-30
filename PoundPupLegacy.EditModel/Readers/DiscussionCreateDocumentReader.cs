namespace PoundPupLegacy.EditModel.Readers;

internal sealed class DiscussionCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<Discussion.ToCreate>
{
    protected override int NodeTypeId => Constants.DISCUSSION;
}
