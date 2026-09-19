using API.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Settings;

public class DeleteSettingRequest
{
    public string Key { get; set; } = string.Empty;
}

public record DeleteSettingResponse(string Message);

public class DeleteSettingEndpoint : Endpoint<DeleteSettingRequest, DeleteSettingResponse>
{
    private readonly AppDbContext _db;

    public DeleteSettingEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Delete("/api/settings/{key}");
        Roles("Admin");

        Summary(s =>
        {
            s.Summary = "Delete a setting";
            s.Description = "Deletes a setting by key.";
        });
    }

    public override async Task HandleAsync(DeleteSettingRequest req, CancellationToken ct)
    {
        var setting = await _db.SystemSettings.FirstOrDefaultAsync(s => s.Key == req.Key, ct);

        if (setting == null)
        {
            ThrowError("Setting key not found.", 404);
        }

        _db.SystemSettings.Remove(setting);
        await _db.SaveChangesAsync(ct);

        Response = new DeleteSettingResponse("Setting deleted successfully.");
    }
}