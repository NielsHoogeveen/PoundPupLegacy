﻿using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Services;

public interface IFetchSearchService
{
    Task<SearchResult> FetchSearch(int limit, int offset, string searchString);
}
