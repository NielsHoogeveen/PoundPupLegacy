﻿using Npgsql;
using NpgsqlTypes;

namespace PoundPupLegacy.Common;

public enum SearchOption
{
    IsEqualTo,
    StartsWith,
    EndsWith,
    Contains
}

public sealed record SearchOptionDatabaseParameter : DatabaseParameter<(string, SearchOption)>
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
        var pattern = GetPattern(value.Item1, value.Item2);
        SetParameter(pattern, command);
    }
}
