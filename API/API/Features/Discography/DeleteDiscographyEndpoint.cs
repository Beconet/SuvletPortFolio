using API.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Discography;

public class DeleteDiscographyRequest
{
    public Guid Id { get; set; }
}

public record DeleteDiscographyResponse(string Message);

public class DeleteDiscographyEndpoint : Endpoint<DeleteDiscographyRequest, DeleteDiscographyResponse>
{
    private readonly AppDbContext _db;

    public DeleteDiscographyEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Delete("/api/discography/{id}");
        Roles("Admin");

        Summary(s =>
        {
            s.Summary = "Delete a discography entry";
            s.Description = "Deletes a discography entry from the database.";
        });
    }

    public override async Task HandleAsync(DeleteDiscographyRequest req, CancellationToken ct)
    {
        var item = await _db.Discography.FirstOrDefaultAsync(d => d.Id == req.Id, ct);

        if (item == null)
        {
            ThrowError("Discography entry not found.", 404);
        }

        _db.Discography.Remove(item);
        await _db.SaveChangesAsync(ct);

        Response = new DeleteDiscographyResponse("Discography entry deleted successfully.");
    }
}