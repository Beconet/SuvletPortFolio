using API.Data;
using API.Shared.Utilities;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Auth;

public record LoginRequest(string Password);
public record LoginResponse(string Token, DateTime ExpiresAt);

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public LoginEndpoint(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public override void Configure()
    {
        Post("/api/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var passwordSetting = await _db.SystemSettings
            .FirstOrDefaultAsync(s => s.Key == "AdminPasswordHash", ct);

        if (passwordSetting == null)
        {
            ThrowError("Admin password is not set in database.", 500);
        }

        if (!PasswordHasher.VerifyPassword(req.Password, passwordSetting.Value))
        {
            ThrowError("Invalid password.", 401);
        }

        var jwtKey = _config["Jwt:SecretKey"] ?? "SUPER_SECRET_KEY_FOR_JWT_SUVLET_2026_PORTFOLIO!";
        var expiresAt = DateTime.UtcNow.AddDays(2);

        var token = JwtBearer.CreateToken(o =>
        {
            o.SigningKey = jwtKey;
            o.ExpireAt = expiresAt;
            o.User.Roles.Add("Admin");
        });

        Response = new LoginResponse(token, expiresAt);
    }
}