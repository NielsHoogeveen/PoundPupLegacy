using PoundPupLegacy.Common;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.Deleters;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.Updaters;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class LocationsSaveService : ISaveService<IEnumerable<Location>>
{
    private readonly IDatabaseDeleterFactory<LocationDeleter> _locationDeleterFactory;
    private readonly IDatabaseUpdaterFactory<LocationUpdater> _locationUpdaterFactory;
    private readonly IEntityCreator<CreateModel.Location> _locationCreator;
    public LocationsSaveService(
        IDatabaseDeleterFactory<LocationDeleter> locationDeleterFactory,
        IDatabaseUpdaterFactory<LocationUpdater> locationUpdaterFactory,
        IEntityCreator<CreateModel.Location> locationCreator
    )
    {
        _locationDeleterFactory = locationDeleterFactory;
        _locationUpdaterFactory = locationUpdaterFactory;
        _locationCreator = locationCreator;
    }
    public async Task SaveAsync(IEnumerable<Location> item, IDbConnection connection)
    {
        await using var deleter = await _locationDeleterFactory.CreateAsync(connection);
        await using var updater = await _locationUpdaterFactory.CreateAsync(connection);
        
        foreach (var location in item.Where(x => x.HasBeenDeleted)) 
        {
            if (!location.LocatableId.HasValue)
                throw new Exception("locatable id of location should be set in order to delete");
            if (!location.LocationId.HasValue)
                throw new Exception("location id of location should be set in order to delete");
            await deleter.DeleteAsync(new LocationDeleter.Request {
                LocatableId = location.LocatableId.Value,
                LocationId = location.LocationId.Value
            });
        }
        foreach (var location in item.Where(x => x.LocationId.HasValue && !x.HasBeenDeleted)) 
        {
            await updater.UpdateAsync(new LocationUpdater.Request 
            {
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
        IEnumerable<CreateModel.Location> GetLocationsToInsert()
        {
            foreach (var location in item.Where(x => !x.LocationId.HasValue)) {
                yield return new CreateModel.Location {
                    Id = null,
                    Street = location.Street,
                    Additional = location.Addition,
                    City = location.City,
                    PostalCode = location.PostalCode,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    SubdivisionId = location.SubdivisionId,
                    CountryId = location.CountryId,
                    Locatables = new List<CreateModel.LocationLocatable>
                    { 
                        new CreateModel.LocationLocatable
                        {
                            LocatableId = location.LocatableId!.Value,
                            LocationId = null,
                        }
                    }
                };
            }
            
        }
        await _locationCreator.CreateAsync(GetLocationsToInsert().ToAsyncEnumerable(), connection);
    }
}