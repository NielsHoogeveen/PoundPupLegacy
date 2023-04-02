using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.EditModel.Readers;

public class DiscussionUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<DiscussionUpdateDocumentReader>
{
    public override async Task<DiscussionUpdateDocumentReader> CreateAsync(IDbConnection connection)
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