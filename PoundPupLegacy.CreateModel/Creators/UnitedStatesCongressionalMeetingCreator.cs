namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class UnitedStatesCongressionalMeetingCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<NewUnitedStatesCongressionalMeeting> unitedStatesCongressionalMeetingInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewUnitedStatesCongressionalMeeting>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewUnitedStatesCongressionalMeeting> meetings, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var unitedStatesCongressionalMeetingWriter = await unitedStatesCongressionalMeetingInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var meeting in meetings) {
            await nodeWriter.InsertAsync(meeting);
            await searchableWriter.InsertAsync(meeting);
            await documentableWriter.InsertAsync(meeting);
            await nameableWriter.InsertAsync(meeting);
            await unitedStatesCongressionalMeetingWriter.InsertAsync(meeting);
            await WriteTerms(meeting, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in meeting.TenantNodes) {
                tenantNode.NodeId = meeting.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
