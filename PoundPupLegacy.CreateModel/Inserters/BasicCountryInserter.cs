namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BasicCountryInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableBasicCountry>
{
    protected override string TableName => "basic_country";

    protected override bool AutoGenerateIdentity => false;

}
