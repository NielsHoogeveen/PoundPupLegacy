#if IOS || ANDROID || MACCATALYST
using Microsoft.Maui.Graphics.Platform;
#elif WINDOWS
using Microsoft.Maui.Graphics.Win2D;
#endif
using System.IO;
using System.Reflection;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace PoundPupLegacy.Common;

internal class ImageResizer
{
}
