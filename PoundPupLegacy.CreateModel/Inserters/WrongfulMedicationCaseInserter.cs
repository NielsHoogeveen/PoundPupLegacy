﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class WrongfulMedicationCaseInserterFactory : SingleIdInserterFactory<WrongfulMedicationCase.WrongfulMedicationCaseToCreate>
{
    protected override string TableName => "wrongful_medication_case";

    protected override bool AutoGenerateIdentity => false;

}
