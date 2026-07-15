namespace backend.DTOs.Auth;

public sealed record AuthResponse(
    string Token,
    DateTime ExpiresAt,
    UserDto User
);
