using PoundPupLegacy.EditModel;

namespace PoundPupLegacy.Services;

public interface ILocationService
{
    Task ProcessLocationAsync(Location location);
}
