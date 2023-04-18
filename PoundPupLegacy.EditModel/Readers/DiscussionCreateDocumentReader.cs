namespace PoundPupLegacy.EditModel.Readers;

internal sealed class DiscussionCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<Discussion>
{
    protected override int NodeTypeId => Constants.DISCUSSION;
}
