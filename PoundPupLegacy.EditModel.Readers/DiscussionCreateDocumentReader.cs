using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public class DiscussionCreateDocumentReader : SimpleTextNodeCreateDocumentReader<Discussion>, ISingleItemDatabaseReader<DiscussionCreateDocumentReader, NodeEditDocumentReader.NodeCreateDocumentRequest, Discussion>
{
    protected DiscussionCreateDocumentReader(NpgsqlCommand command) : base(command, Constants.DISCUSSION)
    {
    }

    public static async Task<DiscussionCreateDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateCommand(connection, SQL);
        return new DiscussionCreateDocumentReader(command);
    }


}


