using API.Shared.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Storage;

public record UploadFileResponse(string FileUrl, string Folder, long FileSizeBytes);

[RequestSizeLimit(100 * 1024 * 1024)] 
public class UploadFileEndpoint : EndpointWithoutRequest<UploadFileResponse>
{
    private readonly R2StorageService _r2Service;

    public UploadFileEndpoint(R2StorageService r2Service) => _r2Service = r2Service;

    public override void Configure()
    {
        Post("/api/storage/upload");
        Roles("Admin");
        AllowFileUploads();

        Summary(s =>
        {
            s.Summary = "Upload audio or cover image file";
            s.Description = "Uploads file directly to Cloudflare R2 (Max size: 100MB). Automatically sorts into folders based on file type.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var file = Files.FirstOrDefault();

        if (file == null || file.Length == 0)
        {
            ThrowError("File is empty or missing.", 400);
        }

        const long maxSizeBytes = 100 * 1024 * 1024; 
        if (file.Length > maxSizeBytes)
        {
            ThrowError($"File upload failed: File size ({file.Length / (1024 * 1024)} MB) exceeds the maximum allowed limit of 100 MB.", 400);
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        var customFolder = Query<string>("folder", isRequired: false) ?? Form["folder"].ToString();

        string folderName = extension switch
        {
            ".wav" or ".mp3" or ".flac" or ".aac" => "demotracks",
            ".jpg" or ".jpeg" or ".png" or ".webp" => "coverimage",
            _ => string.IsNullOrWhiteSpace(customFolder) ? "general" : customFolder
        };

        var fileUrl = await _r2Service.UploadFileAsync(file, folderName, ct);

        Response = new UploadFileResponse(fileUrl, folderName, file.Length);
    }
}