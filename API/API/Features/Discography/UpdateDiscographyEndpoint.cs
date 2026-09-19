using API.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Discography;

public class UpdateDiscographyRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ExternalUrl { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
}

public class UpdateDiscographyEndpoint : Endpoint<UpdateDiscographyRequest, DiscographyResponse>
{
    private readonly AppDbContext _db;

    public UpdateDiscographyEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Put("/api/discography/{id}");
        Roles("Admin");

        Summary(s =>
        {
            s.Summary = "Update a discography entry";
            s.Description = "Updates the details of an existing discography entry.";
        });
    }

    public override async Task HandleAsync(UpdateDiscographyRequest req, CancellationToken ct)
    {
        var item = await _db.Discography.FirstOrDefaultAsync(d => d.Id == req.Id, ct);

        if (item == null)
        {
            ThrowError("Discography entry not found.", 404);
        }

        item.Title = req.Title;
        item.ExternalUrl = req.ExternalUrl;
        item.Role = req.Role;
        item.ReleaseDate = DateTime.SpecifyKind(req.ReleaseDate, DateTimeKind.Utc);
        item.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        Response = new DiscographyResponse(
            item.Id,
            item.Title,
            item.ExternalUrl,
            item.Role,
            item.ReleaseDate,
            item.CreatedAt
        );
    }
}