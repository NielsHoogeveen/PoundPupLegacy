using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.EditModel.Readers;

public class DiscussionCreateDocumentReaderFactory : SimpleTextNodeCreateDocumentReaderFactory<DiscussionCreateDocumentReader>
{
    public override async Task<DiscussionCreateDocumentReader> CreateAsync(IDbConnection connection)
    {
        var command = await CreateCommand(connection, SQL);
        return new DiscussionCreateDocumentReader(command);
    }

}
public class DiscussionCreateDocumentReader : SimpleTextNodeCreateDocumentReader<Discussion>
{
    internal DiscussionCreateDocumentReader(NpgsqlCommand command) : base(command, Constants.DISCUSSION)
    {
    }
}


