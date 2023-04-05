namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ISOCodedFirstLevelSubdivisionInserterFactory : SingleIdInserterFactory<ISOCodedFirstLevelSubdivision>
{
    protected override string TableName => "iso_coded_first_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
