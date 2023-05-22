namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FirstLevelSubdivisionInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableFirstLevelSubdivision>
{
    protected override string TableName => "first_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
