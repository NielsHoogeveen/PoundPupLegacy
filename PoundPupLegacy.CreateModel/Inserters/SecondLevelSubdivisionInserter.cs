namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SecondLevelSubdivisionInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableSecondLevelSubdivision>
{
    protected override string TableName => "second_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
