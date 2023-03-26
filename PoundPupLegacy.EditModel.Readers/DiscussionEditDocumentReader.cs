using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public class DiscussionEditDocumentReader : SimpleTextNodeEditDocumentReader<Discussion>, ISingleItemDatabaseReader<DiscussionEditDocumentReader, NodeEditDocumentReader.NodeEditDocumentRequest, Discussion>
{
    protected DiscussionEditDocumentReader(NpgsqlCommand command) : base(command, Constants.DISCUSSION)
    {
    }

    public static async Task<DiscussionEditDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateCommand(connection, SIMPLE_TEXT_NODE_DOCUMENT);
        return new DiscussionEditDocumentReader(command);
    }


}


