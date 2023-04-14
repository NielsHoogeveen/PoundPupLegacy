namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = PollQuestionInserterFactory;
using Request = PollQuestion;
using Inserter = PollQuestionInserter;

internal sealed class PollQuestionInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableStringDatabaseParameter Question = new() { Name = "question" };

    public override string TableName => "poll_question";
}
internal sealed class PollQuestionInserter : IdentifiableDatabaseInserter<Request>
{
    public PollQuestionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Question, request.Question),
        };
    }
}
