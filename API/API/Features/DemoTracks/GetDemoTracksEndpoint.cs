using API.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace API.Features.DemoTracks;

public class GetDemoTracksEndpoint : EndpointWithoutRequest<List<DemoTrackResponse>>
{
    private readonly AppDbContext _db;

    public GetDemoTracksEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Get("/api/demotracks");
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Get all demo tracks";
            s.Description = "Retrieves all demo tracks available for listening.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var tracks = await _db.DemoTracks
            .AsNoTracking()
            .Select(t => new DemoTrackResponse(
                t.Id,
                t.Name,
                t.Genre,
                t.Description,
                t.AudioUrl,
                t.WaveformDataJson
            ))
            .ToListAsync(ct);

        Response = tracks;
    }
}