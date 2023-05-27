using PoundPupLegacy.CreateModel;
using PoundPupLegacy.CreateModel.Deleters;
using PoundPupLegacy.CreateModel.Updaters;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal class LocationsSaveService(
    IDatabaseDeleterFactory<LocationToDelete> locationDeleterFactory,
    IDatabaseUpdaterFactory<LocationUpdaterRequest> locationUpdaterFactory,
    IEntityCreatorFactory<EventuallyIdentifiableLocation> locationCreatorFactory
) : ISaveService<IEnumerable<Location>>
{
    public async Task SaveAsync(IEnumerable<Location> item, IDbConnection connection)
    {
        await using var deleter = await locationDeleterFactory.CreateAsync(connection);
        await using var updater = await locationUpdaterFactory.CreateAsync(connection);

        foreach (var location in item.Where(x => x.HasBeenDeleted)) {
            if (!location.LocatableId.HasValue)
                throw new Exception("locatable id of location should be set in order to delete");
            if (!location.LocationId.HasValue)
                throw new Exception("location id of location should be set in order to delete");
            await deleter.DeleteAsync(new LocationToDelete {
                LocatableId = location.LocatableId.Value,
                LocationId = location.LocationId.Value
            });
        }
        foreach (var location in item.Where(x => x.LocationId.HasValue && !x.HasBeenDeleted)) {
            await updater.UpdateAsync(new LocationUpdaterRequest {
                Street = location.Street,
                Additional = location.Addition,
                City = location.City,
                PostalCode = location.PostalCode,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                SubdivisionId = location.SubdivisionId,
                CountryId = location.CountryId,
                Id = location.LocationId!.Value
            });
        }
        IEnumerable<EventuallyIdentifiableLocation> GetLocationsToInsert()
        {
            foreach (var location in item.Where(x => !x.LocationId.HasValue)) {
                yield return new CreateModel.NewLocation {
                    Id = null,
                    Street = location.Street,
                    Additional = location.Addition,
                    City = location.City,
                    PostalCode = location.PostalCode,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    SubdivisionId = location.SubdivisionId,
                    CountryId = location.CountryId,
                };
            }

        }
        await using var locationCreator = await locationCreatorFactory.CreateAsync(connection);
        await locationCreator.CreateAsync(GetLocationsToInsert().ToAsyncEnumerable());
    }
}