namespace backend.DTOs.Auth;

public sealed record LoginRequest(
    string Email,
    string Password
);
