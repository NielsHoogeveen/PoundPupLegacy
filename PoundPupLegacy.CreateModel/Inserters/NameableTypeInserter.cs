namespace PoundPupLegacy.CreateModel.Inserters;

using Request = EventuallyIdentifiableNameableType;

internal sealed class NameableTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableStringDatabaseParameter TagLabelName = new() { Name = "tag_label_name" };

    public override string TableName => "nameable_type";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TagLabelName, request.TagLabelName),
        };
    }
}
