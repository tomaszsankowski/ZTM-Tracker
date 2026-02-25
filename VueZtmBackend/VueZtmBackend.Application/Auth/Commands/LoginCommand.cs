using MediatR;

namespace VueZtmBackend.Application.Auth.Commands;

public record LoginCommand(string Login, string Password) : IRequest<LoginResult>;

public record LoginResult(
    bool Success, 
    string? Message, 
    string? AccessToken, 
    string? RefreshToken,
    DateTime? ExpiresAt
);

