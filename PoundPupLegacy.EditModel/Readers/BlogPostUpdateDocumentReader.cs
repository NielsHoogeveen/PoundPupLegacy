﻿namespace PoundPupLegacy.EditModel.Readers;

using Reader = BlogPostUpdateDocumentReader;

public sealed class BlogPostUpdateDocumentReaderFactory : SimpleTextNodeUpdateDocumentReaderFactory<Reader>
{
}

public sealed class BlogPostUpdateDocumentReader : SimpleTextNodeUpdateDocumentReader<BlogPost>
{
    internal BlogPostUpdateDocumentReader(NpgsqlCommand command) : base(command, Constants.BLOG_POST)
    {
    }
}