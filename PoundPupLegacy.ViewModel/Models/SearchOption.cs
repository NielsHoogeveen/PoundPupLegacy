using NpgsqlTypes;

namespace PoundPupLegacy.ViewModel.Models;

public enum SearchOption
{
    IsEqualTo,
    StartsWith,
    EndsWith,
    Contains
}

public record SearchOptionDatabaseParameter : DatabaseParameter<(string, SearchOption)>
{
    public override bool IsNullable => true;
    public override NpgsqlDbType ParameterType => NpgsqlDbType.Varchar;

    public override void Set((string, SearchOption) value, NpgsqlCommand command)
    {
        string GetPattern(string searchTerm, SearchOption searchOption)
        {
            if (string.IsNullOrEmpty(searchTerm)) {
                return "%";
            }
            return searchOption switch {
                SearchOption.IsEqualTo => searchTerm,
                SearchOption.Contains => $"%{searchTerm}%",
                SearchOption.StartsWith => $"{searchTerm}%",
                SearchOption.EndsWith => $"%{searchTerm}",
                _ => throw new Exception("Cannot reach")
            };
        }
        SetParameter(GetPattern(value.Item1, value.Item2), command);
    }
}
