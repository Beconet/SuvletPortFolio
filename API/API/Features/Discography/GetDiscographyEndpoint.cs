using API.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Discography;

public class GetDiscographyEndpoint : EndpointWithoutRequest<List<DiscographyResponse>>
{
    private readonly AppDbContext _db;

    public GetDiscographyEndpoint(AppDbContext db) => _db = db;

    public override void Configure()
    {
        Get("/api/discography");
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Get all discography entries";
            s.Description = "Retrieves all discography entries ordered by release date descending.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var list = await _db.Discography
            .AsNoTracking()
            .OrderByDescending(d => d.ReleaseDate)
            .Select(d => new DiscographyResponse(
                d.Id,
                d.Title,
                d.ExternalUrl,
                d.Role,
                d.ReleaseDate,
                d.CreatedAt
            ))
            .ToListAsync(ct);

        Response = list;
    }
}