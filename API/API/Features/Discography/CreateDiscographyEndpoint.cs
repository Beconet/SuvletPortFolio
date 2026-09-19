using API.Data;
using FastEndpoints;

namespace API.Features.Discography;

public class CreateDiscographyRequest
{
    public string Title { get; set; } = string.Empty;
    public string ExternalUrl { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // e.g. "Producer", "Co-Writer", "Mixing Engineer"
    public DateTime ReleaseDate { get; set; }
}

public record DiscographyResponse(
    Guid Id,
    string Title,
    string ExternalUrl,
    string Role,
    DateTime ReleaseDate,
    DateTime CreatedAt
);

public class CreateDiscographyEndpoint : Endpoint<CreateDiscographyRequest, DiscographyResponse>
{
    private readonly AppDbContext _db;

    public CreateDiscographyEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Post("/api/discography");
        Roles("Admin");

        Summary(s =>
        {
            s.Summary = "Create a new discography entry";
            s.Description = "Adds a new track/project entry to the discography.";
        });
    }

    public override async Task HandleAsync(CreateDiscographyRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Title))
        {
            ThrowError("Title is required.", 400);
        }

        var item = new API.Data.Entities.Discography
        {
            Title = req.Title,
            ExternalUrl = req.ExternalUrl,
            Role = req.Role,
            ReleaseDate = DateTime.SpecifyKind(req.ReleaseDate, DateTimeKind.Utc)
        };

        _db.Discography.Add(item);
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