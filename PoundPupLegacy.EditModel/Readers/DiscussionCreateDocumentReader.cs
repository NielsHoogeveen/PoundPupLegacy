namespace PoundPupLegacy.EditModel.Readers;

using Reader = DiscussionCreateDocumentReader;

public class DiscussionCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<Reader>
{
}
public class DiscussionCreateDocumentReader : SimpleTextNodeCreateDocumentReader<Discussion>
{
    public DiscussionCreateDocumentReader(NpgsqlCommand command) : base(command, Constants.DISCUSSION)
    {
    }
}


