namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(LocatableDetails.ExistingLocatableDetails))]
public partial class ExistingLocatableDetailsJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(LocatableDetails.ExistingLocatableDetails))]
public partial class NewLocatableDetailsJsonContext : JsonSerializerContext { }

public interface Locatable: Nameable
{
    LocatableDetails LocatableDetails { get; }
}
public interface NewLocatable: Locatable, NewNode
{

    LocatableDetails.NewLocatableDetails NewLocatableDetails { get; }
}

public interface ExistingLocatable : Locatable, ExistingNode
{

    LocatableDetails.ExistingLocatableDetails ExistingLocatableDetails { get; }
}


public abstract record LocatableDetails
{
    public abstract IEnumerable<Location> Locations { get; }
    public abstract void Add(NewLocation location);

    private List<CountryListItem> countries = new();

    public List<CountryListItem> Countries {
        get => countries;
        init {
            if (value is not null) {
                countries = value;
            }
        }
    }

    public sealed record NewLocatableDetails: LocatableDetails
    {


        private List<NewLocation> locations = new();

        public List<NewLocation> LocationsToAdd {
            get => locations;
            init {
                if (value is not null) {
                    locations = value;
                }
            }
        }
        public override IEnumerable<Location> Locations => LocationsToAdd;
        public override void Add(NewLocation location)
        {
            LocationsToAdd.Add(location);
        }
    }

    public sealed record ExistingLocatableDetails: LocatableDetails
    {
        private List<NewLocation> locationsToAdd = new();

        public List<NewLocation> LocationsToAdd {
            get => locationsToAdd;
            init {
                if (value is not null) {
                    locationsToAdd = value;
                }
            }
        }

        private List<ExistingLocation> locationsToUpdate = new();

        public List<ExistingLocation> LocationsToUpdate {
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

        public override void Add(NewLocation location)
        {
            LocationsToAdd.Add(location);
        }
    }
}
