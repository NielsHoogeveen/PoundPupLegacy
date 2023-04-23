using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;
using System.Text.RegularExpressions;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed partial class CongressionalDataService : ICongressionalDataService
{


    private record ChamberTypeAndMeetingNumber
    {
        public required ChamberType ChamberType { get; init; }
        public required int Number { get; init; }
    }

    private readonly NpgsqlConnection _connection;
    private readonly ISingleItemDatabaseReaderFactory<UnitedStatesMeetingChamberDocumentReaderRequest, CongressionalMeetingChamber> _unitedStatesMeetingChamberDocumentReaderFactory;
    private readonly ISingleItemDatabaseReaderFactory<UnitedStatesCongresssDocumentReaderRequest, UnitedStatesCongress> _unitedStatesCongresssDocumentReaderFactory;

    public CongressionalDataService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<UnitedStatesMeetingChamberDocumentReaderRequest, CongressionalMeetingChamber> unitedStatesMeetingChamberDocumentReaderFactory,
        ISingleItemDatabaseReaderFactory<UnitedStatesCongresssDocumentReaderRequest, UnitedStatesCongress> unitedStatesCongresssDocumentReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _unitedStatesMeetingChamberDocumentReaderFactory = unitedStatesMeetingChamberDocumentReaderFactory;
        _unitedStatesCongresssDocumentReaderFactory = unitedStatesCongresssDocumentReaderFactory;
    }
    private ChamberTypeAndMeetingNumber? GetChamberTypeAndMeetingNumber(string path)
    {
        var match = MatchIfCongressionalMeetingChamber().Match(path);
        if (!match.Success) {
            return null;

        }
        if (match.Groups.Count != 4) {
            return null;
        }
        var chamberType = match.Groups[1].Value switch {
            "senate" => ChamberType.Senate,
            "house_of_representatives" => ChamberType.House,
            _ => throw new Exception("Cannot reach")
        };
        var number = int.Parse(match.Groups[2].Value);
        return new ChamberTypeAndMeetingNumber {
            ChamberType = chamberType,
            Number = number
        };

    }
    public async Task<CongressionalMeetingChamber?> GetCongressionalMeetingChamber(ChamberType chamberType, int number)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _unitedStatesMeetingChamberDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new UnitedStatesMeetingChamberDocumentReaderRequest {
                Type = (int)chamberType,
                Number = number
            });
        }
        finally {
            await _connection.CloseAsync();
        }
    }

    public async Task<CongressionalMeetingChamber?> GetCongressionalMeetingChamber(string path)
    {
        var congressionalMeetingChamber = GetChamberTypeAndMeetingNumber(path);
        if (congressionalMeetingChamber is null) {
            return null;
        }
        return await GetCongressionalMeetingChamber(congressionalMeetingChamber.ChamberType, congressionalMeetingChamber.Number);
    }

    public async Task<UnitedStatesCongress?> GetUnitedStatesCongress()
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _unitedStatesCongresssDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new UnitedStatesCongresssDocumentReaderRequest());
        }
        finally {
            await _connection.CloseAsync();
        }
    }


    [GeneratedRegex(
        @"united_states_(senate|house_of_representatives)_([0-9]+)(th|st|nd|rd)_congress",
        RegexOptions.CultureInvariant,
        matchTimeoutMilliseconds: 1000)]
    private static partial Regex MatchIfCongressionalMeetingChamber();
}

