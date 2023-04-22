﻿using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.ViewModel.Models;
using System.Drawing;

namespace PoundPupLegacy.Services;

public interface ISiteDataService
{
    (int, string) GetDefaultCountry(int tenantId);
    Task InitializeAsync();

    [RequireNamedArgs]
    string? GetUrlPathForId(int tenantId, int urlId);

    [RequireNamedArgs]
    bool HasAccess(int userId, int tenantId, string path);

    [RequireNamedArgs]
    bool CanEdit(Node node, int userId, int tenantId);

    int GetTenantId(Uri uri);

    int? GetIdForUrlPath(string urlPath, int tenantId);

    [RequireNamedArgs]
    IEnumerable<MenuItem> GetMenuItemsForUser(int userId, int tenantId);

    string GetLogoName(int tenantId);

    string GetSubTitle(int tenantId);

    string GetFooterTitle(int tenantId);

    Link[] GetFooterMenuItems(int tenantId);

    Task RefreshTenants();
}
