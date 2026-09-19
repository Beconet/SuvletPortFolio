using API.Data;
using FastEndpoints;

namespace API.Features.DemoTracks;

public class CreateDemoTrackRequest
{
    public string Name { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AudioUrl { get; set; } = string.Empty;
    public string? WaveformDataJson { get; set; }
}

public record DemoTrackResponse(
    Guid Id,
    string Name,
    string Genre,
    string Description,
    string AudioUrl,
    string? WaveformDataJson
);

public class CreateDemoTrackEndpoint : Endpoint<CreateDemoTrackRequest, DemoTrackResponse>
{
    private readonly AppDbContext _db;

    public CreateDemoTrackEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Post("/api/demotracks");
        Roles("Admin");

        Summary(s =>
        {
            s.Summary = "Create a new demo track";
            s.Description = "Adds a new demo audio track to the portfolio.";
        });
    }

    public override async Task HandleAsync(CreateDemoTrackRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Name))
        {
            ThrowError("Demo track name is required.", 400);
        }

        var demoTrack = new API.Data.Entities.DemoTracks
        {
            Name = req.Name,
            Genre = req.Genre,
            Description = req.Description,
            AudioUrl = req.AudioUrl,
            WaveformDataJson = req.WaveformDataJson
        };

        _db.DemoTracks.Add(demoTrack);
        await _db.SaveChangesAsync(ct);

        Response = new DemoTrackResponse(
            demoTrack.Id,
            demoTrack.Name,
            demoTrack.Genre,
            demoTrack.Description,
            demoTrack.AudioUrl,
            demoTrack.WaveformDataJson
        );
    }
}