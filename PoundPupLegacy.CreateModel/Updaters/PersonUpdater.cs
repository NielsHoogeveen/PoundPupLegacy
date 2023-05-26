namespace PoundPupLegacy.CreateModel.Updaters;

using Request = ImmediatelyIdentifiablePerson;

internal sealed class PersonChangerFactory(
    IDatabaseUpdaterFactory<Request> databaseUpdaterFactory,
    NodeDetailsChangerFactory nodeDetailsChangerFactory) : IEntityChangerFactory<Request>
{
    public async Task<IEntityChanger<Request>> CreateAsync(IDbConnection connection)
    {
        return new NodeChanger<Request>(
            await databaseUpdaterFactory.CreateAsync(connection),
            await nodeDetailsChangerFactory.CreateAsync(connection)
        );
    }
}

internal sealed class PersonUpdaterFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    private static readonly NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    private static readonly NullableDateTimeDatabaseParameter DateOfBirth = new() { Name = "date_of_birth" };
    private static readonly NullableDateTimeDatabaseParameter DateOfDeath = new() { Name = "date_of_death" };
    private static readonly NullableIntegerDatabaseParameter FileIdPortrait = new() { Name = "file_id_portrait" };
    private static readonly NullableStringDatabaseParameter FirstName = new() { Name = "first_name" };
    private static readonly NullableStringDatabaseParameter MiddleName = new() { Name = "middle_name" };
    private static readonly NullableStringDatabaseParameter LastName = new() { Name = "last_name" };
    private static readonly NullableStringDatabaseParameter FullName = new() { Name = "full_name" };
    private static readonly NullableStringDatabaseParameter Suffix = new() { Name = "suffix" };
    private static readonly NullableIntegerDatabaseParameter GovtrackId = new() { Name = "govtrack_id" };
    private static readonly NullableStringDatabaseParameter Bioguide = new() { Name = "bioguide" };



    public override string Sql => $"""
        update node 
        set 
            title=@title
        where id = @node_id;
        update nameable 
        set 
            description=@description
        where id = @node_id;
        update person 
        set 
            date_of_birth=@date_of_birth,
            date_of_death= @date_of_death,
            file_id_portrait=@file_id_portrait,
            first_name= @first_name,
            middle_name=@middle_name,
            last_name= @last_name,
            full_name=@full_name,
            suffix= @suffix,
            govtrack_id=@govtrack_id,
            bioguide= @bioguide
        where id = @node_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(NodeId, request.Id),
            ParameterValue.Create(Title, request.Title),
            ParameterValue.Create(Description, request.Description),
            ParameterValue.Create(DateOfBirth, request.DateOfBirth),
            ParameterValue.Create(DateOfDeath, request.DateOfDeath),
            ParameterValue.Create(FileIdPortrait, request.FileIdPortrait),
            ParameterValue.Create(GovtrackId, request.GovtrackId),
            ParameterValue.Create(FirstName, request.FirstName),
            ParameterValue.Create(MiddleName, request.MiddleName),
            ParameterValue.Create(LastName, request.LastName),
            ParameterValue.Create(Suffix, request.Suffix),
            ParameterValue.Create(FullName, request.FullName),
            ParameterValue.Create(Bioguide, request.Bioguide),
        };
    }
}

