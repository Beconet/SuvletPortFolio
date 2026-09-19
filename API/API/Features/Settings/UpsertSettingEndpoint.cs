using API.Data;
using API.Data.Entities;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Settings;

public class UpsertSettingRequest
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class UpsertSettingEndpoint : Endpoint<UpsertSettingRequest, SettingResponse>
{
    private readonly AppDbContext _db;

    public UpsertSettingEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Put("/api/settings");
        Roles("Admin");

        Summary(s =>
        {
            s.Summary = "Create or update a system setting";
            s.Description = "Upserts a key-value pair in system settings.";
        });
    }

    public override async Task HandleAsync(UpsertSettingRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Key))
        {
            ThrowError("Setting key is required.", 400);
        }

        var setting = await _db.SystemSettings.FirstOrDefaultAsync(s => s.Key == req.Key, ct);

        if (setting == null)
        {
            setting = new SystemSetting
            {
                Key = req.Key,
                Value = req.Value,
                UpdatedAt = DateTime.UtcNow
            };
            _db.SystemSettings.Add(setting);
        }
        else
        {
            setting.Value = req.Value;
            setting.UpdatedAt = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync(ct);

        Response = new SettingResponse(setting.Key, setting.Value, setting.UpdatedAt);
    }
}