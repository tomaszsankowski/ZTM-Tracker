using MediatR;

namespace VueZtmBackend.Application.Auth.Commands;

public record RefreshTokenCommand(string RefreshToken) : IRequest<RefreshTokenResult>;

public record RefreshTokenResult(
    bool Success,
    string? Message,
    string? AccessToken,
    string? RefreshToken,
    DateTime? ExpiresAt
);

