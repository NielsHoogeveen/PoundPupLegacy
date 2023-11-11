using Microsoft.Extensions.Logging;
using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;
using System.Text.RegularExpressions;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed partial class CongressionalDataService(
    NpgsqlDataSource dataSource,
    ILogger<CongressionalDataService> logger,
    ISingleItemDatabaseReaderFactory<UnitedStatesHouseMeetingDocumentReaderRequest, CongressionalMeetingChamber> unitedStatesHouseMeetingDocumentReaderFactory,
    ISingleItemDatabaseReaderFactory<UnitedStatesSenateMeetingDocumentReaderRequest, CongressionalMeetingChamber> unitedStatesSenateMeetingDocumentReaderFactory,
    ISingleItemDatabaseReaderFactory<UnitedStatesMeetingChamberDocumentReaderRequest, CongressionalMeetingChamber> unitedStatesMeetingChamberDocumentReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, UnitedStatesCongress> unitedStatesCongresssDocumentReaderFactory
) : DatabaseService(dataSource, logger), ICongressionalDataService
{
    public async Task<CongressionalMeetingChamber?> GetSenateMeeting(int number, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await unitedStatesSenateMeetingDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new UnitedStatesSenateMeetingDocumentReaderRequest {
                Number = number,
                TenantId = tenantId
            });
        });
    }
    public async Task<CongressionalMeetingChamber?> GetHouseMeeting(int number, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await unitedStatesHouseMeetingDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new UnitedStatesHouseMeetingDocumentReaderRequest {
                Number = number,
                TenantId = tenantId
            });
        });
    }

    private record ChamberTypeAndMeetingNumber
    {
        public required ChamberType ChamberType { get; init; }
        public required int Number { get; init; }
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
    public async Task<CongressionalMeetingChamber?> GetCongressionalMeetingChamber(ChamberType chamberType, int number, int tenantId)
    {

        return await WithConnection(async (connection) => 
        {
            await using var reader = await unitedStatesMeetingChamberDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new UnitedStatesMeetingChamberDocumentReaderRequest {
                Type = (int)chamberType,
                Number = number,
                TenantId = tenantId
            });
        });
    }

    public async Task<CongressionalMeetingChamber?> GetCongressionalMeetingChamber(string path, int tenantId)
    {
        var congressionalMeetingChamber = GetChamberTypeAndMeetingNumber(path);
        if (congressionalMeetingChamber is null) {
            return null;
        }
        return await GetCongressionalMeetingChamber(congressionalMeetingChamber.ChamberType, congressionalMeetingChamber.Number, tenantId);
    }

    public async Task<UnitedStatesCongress?> GetUnitedStatesCongress(int tenantId, int userId)
    {

        return await WithConnection(async (connection) => {
            await using var reader = await unitedStatesCongresssDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeDocumentReaderRequest { 
                NodeId  = 100237,
                TenantId = tenantId,
                UserId = userId
            });
        });
    }


    [GeneratedRegex(
        @"united_states_(senate|house_of_representatives)_([0-9]+)(th|st|nd|rd)_congress",
        RegexOptions.CultureInvariant,
        matchTimeoutMilliseconds: 1000)]
    private static partial Regex MatchIfCongressionalMeetingChamber();
}

