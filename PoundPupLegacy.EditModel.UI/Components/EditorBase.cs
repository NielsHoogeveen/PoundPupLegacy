using Microsoft.AspNetCore.Components.Forms;
using PoundPupLegacy.Common.UI.Components;

namespace PoundPupLegacy.EditModel.UI.Components;

public abstract class EditorBase: ViewerBase
{
    public abstract Task Validate(ValidationMessageStore validationMessageStore, List<string> invalidIds);
}
