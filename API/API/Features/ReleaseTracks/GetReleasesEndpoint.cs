using API.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace API.Features.ReleaseTracks;

public class GetReleasesEndpoint : EndpointWithoutRequest<List<ReleaseResponse>>
{
    private readonly AppDbContext _db;

    public GetReleasesEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Get("/api/releases");
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Get all releases";
            s.Description = "Retrieves all music releases ordered by release date descending.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var releases = await _db.Releases
            .OrderByDescending(r => r.ReleaseDate)
            .Select(r => new ReleaseResponse(
                r.Id,
                r.Title,
                r.Formats,
                r.Description,
                r.CoverImageUrl,
                r.SpotifyUrl,
                r.AppleMusicUrl,
                r.YoutubeUrl,
                r.ReleaseDate,
                r.CreatedAt
            ))
            .ToListAsync(ct);

        Response = releases;
    }
}