using MediatR;

namespace VueZtmBackend.Application.Auth.Commands;

public record RegisterCommand(string Login, string Password) : IRequest<RegisterResult>;

public record RegisterResult(bool Success, string? Message, Guid? UserId);
