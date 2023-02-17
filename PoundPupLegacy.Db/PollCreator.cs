namespace PoundPupLegacy.Db;

public class PollCreator : IEntityCreator<Poll>
{
    public static async Task CreateAsync(IAsyncEnumerable<Poll> blogPosts, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeWriter.CreateAsync(connection);
        await using var pollWriter = await PollWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);
        await using var pollOptionWriter = await PollOptionWriter.CreateAsync(connection);
        await using var pollVoteWriter = await PollVoteWriter.CreateAsync(connection);

        await foreach (var blogPost in blogPosts)
        {
            await nodeWriter.WriteAsync(blogPost);
            await searchableWriter.WriteAsync(blogPost);
            await simpleTextNodeWriter.WriteAsync(blogPost);
            await pollWriter.WriteAsync(blogPost);
            foreach (var tenantNode in blogPost.TenantNodes)
            {
                tenantNode.NodeId = blogPost.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }
            foreach(var pollOption in blogPost.PollOptions)
            {
                pollOption.PollId = blogPost.Id;
                await pollOptionWriter.WriteAsync(pollOption);
            }
            foreach (var pollVote in blogPost.PollVotes)
            {
                pollVote.PollId = blogPost.Id;
                await pollVoteWriter.WriteAsync(pollVote);
            }

        }
    }
}
