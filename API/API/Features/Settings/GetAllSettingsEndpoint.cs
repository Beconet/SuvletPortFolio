using API.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Settings;

public class GetAllSettingsEndpoint : EndpointWithoutRequest<List<SettingResponse>>
{
    private readonly AppDbContext _db;

    public GetAllSettingsEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Get("/api/settings");
        Roles("Admin");

        Summary(s =>
        {
            s.Summary = "Get all system settings";
            s.Description = "Retrieves all key-value settings.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var settings = await _db.SystemSettings
            .AsNoTracking()
            .Select(s => new SettingResponse(s.Key, s.Value, s.UpdatedAt))
            .ToListAsync(ct);

        Response = settings;
    }
}