using Microsoft.AspNetCore.Components;

namespace PoundPupLegacy.Common.UI.Components;

public abstract class ViewerBase: ComponentBase
{
    [CascadingParameter(Name = "User")]
    public User User { get; set; } = default!;

    [CascadingParameter(Name = "Tenant")]
    public ITenant Tenant { get; set; } = default!;

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
