using API.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace API.Features.ReleaseTracks;

public class DeleteReleaseRequest
{
    public Guid Id { get; set; }
}

public record DeleteReleaseResponse(string Message);

public class DeleteReleaseEndpoint : Endpoint<DeleteReleaseRequest, DeleteReleaseResponse>
{
    private readonly AppDbContext _db;

    public DeleteReleaseEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Delete("/api/releases/{id}");
        Roles("Admin");
    }

    public override async Task HandleAsync(DeleteReleaseRequest req, CancellationToken ct)
    {
        var release = await _db.Releases.FirstOrDefaultAsync(r => r.Id == req.Id, ct);

        if (release == null)
        {
            ThrowError("Release not found.", 404);
        }

        _db.Releases.Remove(release);
        await _db.SaveChangesAsync(ct);

        Response = new DeleteReleaseResponse("Release deleted successfully.");
    }
}