using API.Shared.Services;
using FastEndpoints;

namespace API.Features.Storage;

public class DeleteFileRequest
{
    [QueryParam]
    public string FileUrl { get; set; } = string.Empty;
}

public record DeleteFileResponse(string Message);

public class DeleteFileEndpoint : Endpoint<DeleteFileRequest, DeleteFileResponse>
{
    private readonly R2StorageService _r2Service;

    public DeleteFileEndpoint(R2StorageService r2Service) => _r2Service = r2Service;

    public override void Configure()
    {
        Delete("/api/storage/delete");
        Roles("Admin");

        Summary(s =>
        {
            s.Summary = "Delete a file from Cloudflare R2";
            s.Description = "Deletes an existing file using its public R2 URL.";
        });
    }

    public override async Task HandleAsync(DeleteFileRequest req, CancellationToken ct)
    {
        var url = !string.IsNullOrWhiteSpace(req.FileUrl)
            ? req.FileUrl
            : Query<string>("fileUrl", isRequired: false) ?? Form["fileUrl"].ToString();

        if (string.IsNullOrWhiteSpace(url))
        {
            ThrowError("File URL is required.", 400);
        }

        var isDeleted = await _r2Service.DeleteFileAsync(url, ct);

        if (!isDeleted)
        {
            ThrowError("Failed to delete file or file not found.", 400);
        }

        Response = new DeleteFileResponse("File deleted successfully.");
    }
}