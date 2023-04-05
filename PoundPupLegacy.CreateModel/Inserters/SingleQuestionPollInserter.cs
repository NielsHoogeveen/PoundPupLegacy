﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SingleQuestionPollInserterFactory : SingleIdInserterFactory<SingleQuestionPoll>
{
    protected override string TableName => "single_question_poll";

    protected override bool AutoGenerateIdentity => false;

}
