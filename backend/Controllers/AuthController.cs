using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using backend.DTOs.Auth;
using backend.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<RegisterRequest> _registerValidator;
    private readonly IValidator<LoginRequest> _loginValidator;

    public AuthController(
        IAuthService authService,
        IValidator<RegisterRequest> registerValidator,
        IValidator<LoginRequest> loginValidator)
    {
        _authService = authService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    /// <summary>Register a new user account.</summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var validation = await _registerValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail(
                validation.Errors.Select(e => e.ErrorMessage)));

        var result = await _authService.RegisterAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Me), new { }, ApiResponse<AuthResponse>.Ok(result));
    }

    /// <summary>Authenticate with email and password.</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var validation = await _loginValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail(
                validation.Errors.Select(e => e.ErrorMessage)));

        var result = await _authService.LoginAsync(request, cancellationToken);
        return Ok(ApiResponse<AuthResponse>.Ok(result));
    }

    /// <summary>Get the currently authenticated user's profile.</summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var userId = GetUserIdFromClaims();
        if (userId == Guid.Empty)
            return Unauthorized(ApiResponse<object>.Fail(["Invalid token payload."]));

        var user = await _authService.GetCurrentUserAsync(userId, cancellationToken);
        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    /// <summary>Logout (client-side token invalidation).</summary>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public IActionResult Logout()
    {
        return Ok(ApiResponse<object>.Ok(null, "Logged out successfully."));
    }

    private Guid GetUserIdFromClaims()
    {
        var subClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                    ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Guid.TryParse(subClaim, out var userId) ? userId : Guid.Empty;
    }
}

/// <summary>Consistent API envelope for all responses.</summary>
public sealed class ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string Message { get; init; } = string.Empty;
    public IEnumerable<string> Errors { get; init; } = [];
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    public static ApiResponse<T> Ok(T? data, string message = "Request completed successfully.") =>
        new() { Success = true, Data = data, Message = message };

    public static ApiResponse<T> Fail(IEnumerable<string> errors, string message = "Request failed.") =>
        new() { Success = false, Errors = errors, Message = message };
}
