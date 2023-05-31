namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(LocatableDetails.ForUpdate))]
public partial class ExistingLocatableDetailsJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(LocatableDetails.ForUpdate))]
public partial class NewLocatableDetailsJsonContext : JsonSerializerContext { }

public interface Locatable: Nameable
{
    LocatableDetails LocatableDetails { get; }
}
public interface NewLocatable: Locatable, NewNode
{

    LocatableDetails.ForCreate LocatableDetailsForCreate { get; }
}

public interface ExistingLocatable : Locatable, ExistingNode
{

    LocatableDetails.ForUpdate LocatableDetailsForUpdate { get; }
}


public abstract record LocatableDetails
{
    public abstract IEnumerable<Location> Locations { get; }
    public abstract void Add(Location.ToCreate location);

    private List<CountryListItem> countries = new();

    public List<CountryListItem> Countries {
        get => countries;
        init {
            if (value is not null) {
                countries = value;
            }
        }
    }

    public sealed record ForCreate: LocatableDetails
    {


        private List<Location.ToCreate> locations = new();

        public List<Location.ToCreate> LocationsToAdd {
            get => locations;
            init {
                if (value is not null) {
                    locations = value;
                }
            }
        }
        public override IEnumerable<Location> Locations => LocationsToAdd;
        public override void Add(Location.ToCreate location)
        {
            LocationsToAdd.Add(location);
        }
    }

    public sealed record ForUpdate: LocatableDetails
    {
        private List<Location.ToCreate> locationsToAdd = new();

        public List<Location.ToCreate> LocationsToAdd {
            get => locationsToAdd;
            init {
                if (value is not null) {
                    locationsToAdd = value;
                }
            }
        }

        private List<Location.ToUpdate> locationsToUpdate = new();

        public List<Location.ToUpdate> LocationsToUpdate {
            get => locationsToUpdate;
            init {
                if (value is not null) {
                    locationsToUpdate = value;
                }
            }
        }
        public override IEnumerable<Location> Locations => GetLocations();
        private IEnumerable<Location> GetLocations()
        {
            foreach (var location in locationsToAdd) {
                yield return location;
            }
            foreach (var location in locationsToUpdate) {
                yield return location;
            }
        }

        public override void Add(Location.ToCreate location)
        {
            LocationsToAdd.Add(location);
        }
    }
}
