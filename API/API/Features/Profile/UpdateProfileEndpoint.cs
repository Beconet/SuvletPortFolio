using API.Data;
using API.Data.Entities;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Profile;

public class UpdateProfileInfoRequest
{
    public string Description { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? InstagramUrl { get; set; }
    public string? SpotifyUrl { get; set; }
    public string? SoundcloudUrl { get; set; }
    public string? YoutubeUrl { get; set; }
}

public class UpdateProfileEndpoint : Endpoint<UpdateProfileInfoRequest, ProfileInfoResponse>
{
    private readonly AppDbContext _db;

    public UpdateProfileEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Put("/api/profile");
        Roles("Admin");

        Summary(s =>
        {
            s.Summary = "Update artist profile info";
            s.Description = "Updates profile description, email, and social media links.";
        });
    }

    public override async Task HandleAsync(UpdateProfileInfoRequest req, CancellationToken ct)
    {
        var profile = await _db.ProfileInfo.FirstOrDefaultAsync(ct);

        if (profile == null)
        {
            profile = new ProfileInfo();
            _db.ProfileInfo.Add(profile);
        }

        profile.Description = req.Description;
        profile.Email = req.Email;
        profile.InstagramUrl = req.InstagramUrl;
        profile.SpotifyUrl = req.SpotifyUrl;
        profile.SoundcloudUrl = req.SoundcloudUrl;
        profile.YoutubeUrl = req.YoutubeUrl;

        await _db.SaveChangesAsync(ct);

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