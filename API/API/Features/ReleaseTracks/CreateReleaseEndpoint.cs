using API.Data;
using FastEndpoints;

namespace API.Features.ReleaseTracks;

public class CreateReleaseRequest
{
    public string Title { get; set; } = string.Empty;
    public string Formats { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public string? SpotifyUrl { get; set; }
    public string? AppleMusicUrl { get; set; }
    public string? YoutubeUrl { get; set; }
    public DateTime ReleaseDate { get; set; }
}

public record ReleaseResponse(
    Guid Id,
    string Title,
    string Formats,
    string Description,
    string CoverImageUrl,
    string? SpotifyUrl,
    string? AppleMusicUrl,
    string? YoutubeUrl,
    DateTime ReleaseDate,
    DateTime CreatedAt
);

public class CreateReleaseEndpoint : Endpoint<CreateReleaseRequest, ReleaseResponse>
{
    private readonly AppDbContext _db;

    public CreateReleaseEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Post("/api/releases");
        Roles("Admin");

        Summary(s =>
        {
            s.Summary = "Create a new release";
            s.Description = "Adds a new music release (Single, EP, Album) to the portfolio.";
        });
    }

    public override async Task HandleAsync(CreateReleaseRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Title))
        {
            ThrowError("Release title is required.", 400);
        }

        var release = new API.Data.Entities.Releases
        {
            Title = req.Title,
            Formats = req.Formats,
            Description = req.Description,
            CoverImageUrl = req.CoverImageUrl,
            SpotifyUrl = req.SpotifyUrl,
            AppleMusicUrl = req.AppleMusicUrl,
            YoutubeUrl = req.YoutubeUrl,
            ReleaseDate = DateTime.SpecifyKind(req.ReleaseDate, DateTimeKind.Utc)
        };

        _db.Releases.Add(release);
        await _db.SaveChangesAsync(ct);

        Response = new ReleaseResponse(
            release.Id,
            release.Title,
            release.Formats,
            release.Description,
            release.CoverImageUrl,
            release.SpotifyUrl,
            release.AppleMusicUrl,
            release.YoutubeUrl,
            release.ReleaseDate,
            release.CreatedAt
        );
    }
}