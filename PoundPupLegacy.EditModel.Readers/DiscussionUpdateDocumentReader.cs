using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public class DiscussionUpdateDocumentReader : SimpleTextNodeUpdateDocumentReader<Discussion>, ISingleItemDatabaseReader<DiscussionUpdateDocumentReader, NodeEditDocumentReader.NodeUpdateDocumentRequest, Discussion>
{
    protected DiscussionUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.DISCUSSION)
    {
    }

    public static async Task<DiscussionUpdateDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateCommand(connection, SQL);
        return new DiscussionUpdateDocumentReader(command);
    }


}


