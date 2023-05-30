﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SingleQuestionPollInserterFactory : SingleIdInserterFactory<SingleQuestionPoll.ToCreate>
{
    protected override string TableName => "single_question_poll";

    protected override bool AutoGenerateIdentity => false;

}
