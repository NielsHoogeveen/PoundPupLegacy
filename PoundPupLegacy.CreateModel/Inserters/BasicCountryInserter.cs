namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BasicCountryInserterFactory : SingleIdInserterFactory<BasicCountry.ToCreate>
{
    protected override string TableName => "basic_country";

    protected override bool AutoGenerateIdentity => false;

}
