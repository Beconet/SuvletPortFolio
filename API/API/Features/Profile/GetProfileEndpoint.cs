using API.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Profile;

public record ProfileInfoResponse(
    Guid Id,
    string Description,
    string Email,
    string? InstagramUrl,
    string? SpotifyUrl,
    string? SoundcloudUrl,
    string? YoutubeUrl
);

public class GetProfileEndpoint : EndpointWithoutRequest<ProfileInfoResponse>
{
    private readonly AppDbContext _db;

    public GetProfileEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Get("/api/profile");
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Get artist profile info";
            s.Description = "Retrieves profile description, email, and social media links.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var profile = await _db.ProfileInfo.AsNoTracking().FirstOrDefaultAsync(ct);

        if (profile == null)
        {
            Response = new ProfileInfoResponse(
                Guid.Empty,
                "Suvlet - Music Producer",
                "contact@suvlet.com",
                null, null, null, null
            );
            return;
        }

        Response = new ProfileInfoResponse(
            profile.Id,
            profile.Description,
            profile.Email,
            profile.InstagramUrl,
            profile.SpotifyUrl,
            profile.SoundcloudUrl,
            profile.YoutubeUrl
        );
    }
}