using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;
using System.Text.RegularExpressions;

namespace PoundPupLegacy.Services.Implementation;
internal sealed partial class CongressionalDataService : ICongressionalDataService
{

    private enum ChamberType
    {
        House = Constants.UNITED_STATES_HOUSE_OF_REPRESENTATIVES,
        Senate = Constants.UNITED_STATES_SENATE
    }

    private record ChamberTypeAndMeetingNumber
    {
        public required ChamberType ChamberType { get; init; }
        public required int Number { get; init; }
    }

    private readonly NpgsqlConnection _connection;
    private readonly ILogger<CongressionalDataService> _logger;
    private readonly IRazorViewToStringService _razorViewToStringService;
    private readonly ISingleItemDatabaseReaderFactory<UnitedStatesMeetingChamberDocumentReaderRequest, CongressionalMeetingChamber> _unitedStatesMeetingChamberDocumentReaderFactory;
    private readonly ISingleItemDatabaseReaderFactory<UnitedStatesCongresssDocumentReaderRequest, UnitedStatesCongress> _unitedStatesCongresssDocumentReaderFactory;

    public CongressionalDataService(
        IDbConnection connection,
        ILogger<CongressionalDataService> logger,
        IRazorViewToStringService razorViewToStringService,
        ISingleItemDatabaseReaderFactory<UnitedStatesMeetingChamberDocumentReaderRequest, CongressionalMeetingChamber> unitedStatesMeetingChamberDocumentReaderFactory,
        ISingleItemDatabaseReaderFactory<UnitedStatesCongresssDocumentReaderRequest, UnitedStatesCongress> unitedStatesCongresssDocumentReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _logger = logger;
        _razorViewToStringService = razorViewToStringService;
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

    public async Task<CongressionalMeetingChamber?> GetCongressionalMeetingChamber(string path)
    {
        var congressionalMeetingChamber = GetChamberTypeAndMeetingNumber(path);
        if (congressionalMeetingChamber is null) {
            return null;
        }
        try {
            await _connection.OpenAsync();
            await using var reader = await _unitedStatesMeetingChamberDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new UnitedStatesMeetingChamberDocumentReaderRequest {
                Type = (int)congressionalMeetingChamber.ChamberType,
                Number = congressionalMeetingChamber.Number
            });
        }
        finally {
            await _connection.CloseAsync();
        }

    }

    public async Task<string?> GetCongressionalMeetingChamberResult(HttpContext context)
    {
        var congressionalMeetingChamber = GetCongressionalMeetingChamber(context.Request.Path);
        if (congressionalMeetingChamber is null) {
            return null;
        }
        return await _razorViewToStringService.GetFromView("/Views/Shared/CongressionalMeetingChamber.cshtml", congressionalMeetingChamber, context);
    }

    public async Task<string?> GetUnitedStatesCongress(HttpContext context)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _unitedStatesCongresssDocumentReaderFactory.CreateAsync(_connection);
            var document = await reader.ReadAsync(new UnitedStatesCongresssDocumentReaderRequest());
            return await _razorViewToStringService.GetFromView("/Views/Shared/UnitedStatesCongress.cshtml", document, context);
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

