using Microsoft.AspNetCore.Components;

namespace PoundPupLegacy.Common.UI.Components;

public abstract class ViewerBase: ComponentBase
{
    [CascadingParameter(Name = "UserId")]
    public int UserId { get; set; }

    [CascadingParameter(Name = "TenantId")]
    public int TenantId { get; set; }

    [CascadingParameter(Name = "SiteName")]
    public string? SiteName { get; set; }
    protected virtual Link[] BreadCrumElements => new Link[]
    {
        new BasicLink
        {
            Title = "Home",
            Path = "/"
        }
    };

}
