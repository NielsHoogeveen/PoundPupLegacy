using Microsoft.EntityFrameworkCore.Query.Internal;

namespace PoundPupLegacy.Services;

public interface IImagingService
{
    byte[] Resize(Stream stream, int height);
}
