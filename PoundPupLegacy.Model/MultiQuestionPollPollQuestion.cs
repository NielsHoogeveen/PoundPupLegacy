﻿namespace PoundPupLegacy.Model;

public record MultiQuestionPollPollQuestion
{
    public required int MultiQuestionPollId { get; init; }
    public required int PollQuestionId { get; init; }
    public required int Delta { get; init; }
}