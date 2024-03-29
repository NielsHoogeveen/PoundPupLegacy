﻿using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

using Request = File;

internal sealed class FileInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request>
{
    private static readonly NonNullableStringDatabaseParameter Path = new() { Name = "path" };
    private static readonly NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    private static readonly NonNullableStringDatabaseParameter MimeType = new() { Name = "mime_type" };
    private static readonly NonNullableIntegerDatabaseParameter Size = new() { Name = "size" };

    public override string TableName => "file";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Path, request.Path),
            ParameterValue.Create(Name, request.Name),
            ParameterValue.Create(MimeType, request.MimeType),
            ParameterValue.Create(Size, request.Size)
        };
    }

}

