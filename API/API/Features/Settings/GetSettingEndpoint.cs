using API.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Settings;

public class GetSettingRequest
{
    public string Key { get; set; } = string.Empty;
}

public record SettingResponse(string Key, string Value, DateTime UpdatedAt);

public class GetSettingEndpoint : Endpoint<GetSettingRequest, SettingResponse>
{
    private readonly AppDbContext _db;

    public GetSettingEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Get("/api/settings/{key}");
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Get setting value by key";
            s.Description = "Retrieves a system setting value.";
        });
    }

    public override async Task HandleAsync(GetSettingRequest req, CancellationToken ct)
    {
        var setting = await _db.SystemSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Key == req.Key, ct);

        if (setting == null)
        {
            ThrowError("Setting key not found.", 404);
        }

        Response = new SettingResponse(setting.Key, setting.Value, setting.UpdatedAt);
    }
}