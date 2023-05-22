﻿namespace PoundPupLegacy.CreateModel.Inserters;

using Request = EventuallyIdentifiablePollQuestion;

internal sealed class PollQuestionInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableStringDatabaseParameter Question = new() { Name = "question" };

    public override string TableName => "poll_question";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Question, request.Question),
        };
    }
}
