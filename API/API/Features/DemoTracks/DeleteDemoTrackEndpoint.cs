using API.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace API.Features.DemoTracks;

public class DeleteDemoTrackRequest
{
    public Guid Id { get; set; }
}

public record DeleteDemoTrackResponse(string Message);

public class DeleteDemoTrackEndpoint : Endpoint<DeleteDemoTrackRequest, DeleteDemoTrackResponse>
{
    private readonly AppDbContext _db;

    public DeleteDemoTrackEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Delete("/api/demotracks/{id}");
        Roles("Admin");

        Summary(s =>
        {
            s.Summary = "Delete a demo track";
            s.Description = "Deletes a demo track from the database.";
        });
    }

    public override async Task HandleAsync(DeleteDemoTrackRequest req, CancellationToken ct)
    {
        var track = await _db.DemoTracks.FirstOrDefaultAsync(t => t.Id == req.Id, ct);

        if (track == null)
        {
            ThrowError("Demo track not found.", 404);
        }

        _db.DemoTracks.Remove(track);
        await _db.SaveChangesAsync(ct);

        Response = new DeleteDemoTrackResponse("Demo track deleted successfully.");
    }
}