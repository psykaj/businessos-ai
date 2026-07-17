using backend.Persistence;
using backend.DTOs.Auth;
using backend.Entities;
using backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public sealed class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(ApplicationDbContext context, IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        var emailExists = await _context.Users
            .AnyAsync(u => u.Email == request.Email.ToLower(), cancellationToken);

        if (emailExists)
            throw new InvalidOperationException("An account with this email address already exists.");

        var organization = new backend.Entities.Organization
        {
            Name = $"{request.FullName}'s Organization",
            Slug = request.Email.Split('@')[0].ToLower() + "-" + Guid.NewGuid().ToString().Substring(0, 8),
            Email = request.Email.ToLower().Trim(),
            SubscriptionPlan = "Free",
            SubscriptionStatus = "Active"
        };

        _context.Organizations.Add(organization);

        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = request.Email.ToLower().Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "Admin", // First user in org is Admin
            IsActive = true,
            Organization = organization
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return BuildAuthResponse(user);
    }

    public async Task<AuthResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email.ToLower(), cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Your account has been deactivated. Please contact support.");

        return BuildAuthResponse(user);
    }

    public async Task<UserDto> GetCurrentUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
            throw new KeyNotFoundException("User not found.");

        return MapToDto(user);
    }

    private AuthResponse BuildAuthResponse(User user)
    {
        var token = _jwtTokenGenerator.GenerateToken(user);
        var expiresAt = _jwtTokenGenerator.GetExpirationDate();
        return new AuthResponse(token, expiresAt, MapToDto(user));
    }

    private static UserDto MapToDto(User user) =>
        new(user.Id, user.FullName, user.Email, user.Role, user.IsActive, user.CreatedAt);
}
