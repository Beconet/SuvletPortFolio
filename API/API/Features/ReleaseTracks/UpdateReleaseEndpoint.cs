using API.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace API.Features.ReleaseTracks;

public class UpdateReleaseRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Formats { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public string? SpotifyUrl { get; set; }
    public string? AppleMusicUrl { get; set; }
    public string? YoutubeUrl { get; set; }
    public DateTime ReleaseDate { get; set; }
}

public class UpdateReleaseEndpoint : Endpoint<UpdateReleaseRequest, ReleaseResponse>
{
    private readonly AppDbContext _db;

    public UpdateReleaseEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Put("/api/releases/{id}");
        Roles("Admin");
    }

    public override async Task HandleAsync(UpdateReleaseRequest req, CancellationToken ct)
    {
        var release = await _db.Releases.FirstOrDefaultAsync(r => r.Id == req.Id, ct);

        if (release == null)
        {
            ThrowError("Release not found.", 404);
        }

        release.Title = req.Title;
        release.Formats = req.Formats;
        release.Description = req.Description;
        release.CoverImageUrl = req.CoverImageUrl;
        release.SpotifyUrl = req.SpotifyUrl;
        release.AppleMusicUrl = req.AppleMusicUrl;
        release.YoutubeUrl = req.YoutubeUrl;
        release.ReleaseDate = DateTime.SpecifyKind(req.ReleaseDate, DateTimeKind.Utc);
        release.UpdatedAt = DateTime.UtcNow;

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