using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public class DiscussionUpdateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<DiscussionUpdateDocumentReader>
{
    public override async Task<DiscussionUpdateDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateCommand(connection, SQL);
        return new DiscussionUpdateDocumentReader(command);
    }

}

public class DiscussionUpdateDocumentReader : SimpleTextNodeUpdateDocumentReader<Discussion>
{
    internal DiscussionUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.ARTICLE)
    {
    }
}