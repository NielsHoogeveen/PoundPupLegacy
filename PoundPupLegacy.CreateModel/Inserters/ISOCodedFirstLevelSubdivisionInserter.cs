namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ISOCodedFirstLevelSubdivisionInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableISOCodedFirstLevelSubdivision>
{
    protected override string TableName => "iso_coded_first_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
