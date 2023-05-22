namespace PoundPupLegacy.Common.Test;

public class FuzzyDateTests
{
    public class Document
    {
        public required int Id { get; init; }
        public required FuzzyDate Published { get; init; }
    }
    [Fact]
    public void FuzzyDateJsonRegexMatchesOnCorrectInput()
    {
        var regex = FuzzyDate.FuzzyDateJsonRegex();
        var m = regex.Match(@"[""2021-01-01 00:00:00"",""2021-01-01 23:59:59.999"")");
        Assert.True(m.Success);
    }
    [Fact]
    public async Task FuzzyDateGetsDeserializedCorrectly()
    {
        

        var connection = DatabaseValidatorBase.GetConnection();
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT jsonb_build_object('Id', id,'Published', published) FROM document where published is not null LIMIT 1 OFFSET 0";
        var reader = await command.ExecuteReaderAsync();
        if(reader.Read()) {
            var document = reader.GetFieldValue<Document>(0);
            Assert.NotNull(document.Published);

        }
    }
}
