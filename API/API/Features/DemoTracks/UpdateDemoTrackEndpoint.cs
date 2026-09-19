using API.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace API.Features.DemoTracks;

public class UpdateDemoTrackRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AudioUrl { get; set; } = string.Empty;
    public string? WaveformDataJson { get; set; }
}

public class UpdateDemoTrackEndpoint : Endpoint<UpdateDemoTrackRequest, DemoTrackResponse>
{
    private readonly AppDbContext _db;

    public UpdateDemoTrackEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Put("/api/demotracks/{id}");
        Roles("Admin");

        Summary(s =>
        {
            s.Summary = "Update a demo track";
            s.Description = "Updates the details of a demo track.";
        });
    }

    public override async Task HandleAsync(UpdateDemoTrackRequest req, CancellationToken ct)
    {
        var track = await _db.DemoTracks.FirstOrDefaultAsync(t => t.Id == req.Id, ct);

        if (track == null)
        {
            ThrowError("Demo track not found.", 404);
        }

        track.Name = req.Name;
        track.Genre = req.Genre;
        track.Description = req.Description;
        track.AudioUrl = req.AudioUrl;
        track.WaveformDataJson = req.WaveformDataJson;

        await _db.SaveChangesAsync(ct);

        Response = new DemoTrackResponse(
            track.Id,
            track.Name,
            track.Genre,
            track.Description,
            track.AudioUrl,
            track.WaveformDataJson
        );
    }
}