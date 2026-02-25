using MediatR;
using VueZtmBackend.Application.Auth.Commands;
using VueZtmBackend.Application.Common.Interfaces;
using VueZtmBackend.Domain.Interfaces;
using VueZtmBackend.Domain.ValueObjects;

namespace VueZtmBackend.Application.Auth.Handlers;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public RefreshTokenHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return new RefreshTokenResult(false, "Refresh token jest wymagany.", null, null, null);
        }

        var user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (user is null || user.RefreshToken is null)
        {
            return new RefreshTokenResult(false, "Nieprawidłowy refresh token.", null, null, null);
        }

        if (user.RefreshToken.IsExpired)
        {
            user.UpdateRefreshToken(null);
            await _userRepository.UpdateAsync(user, cancellationToken);
            return new RefreshTokenResult(false, "Refresh token wygasł. Zaloguj się ponownie.", null, null, null);
        }

        var accessToken = _jwtService.GenerateAccessToken(user);
        var newRefreshTokenValue = _jwtService.GenerateRefreshToken();
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

        var newRefreshToken = RefreshToken.Create(newRefreshTokenValue, refreshTokenExpiry);
        user.UpdateRefreshToken(newRefreshToken);
        await _userRepository.UpdateAsync(user, cancellationToken);

        return new RefreshTokenResult(
            true,
            "Token odświeżony pomyślnie.",
            accessToken,
            newRefreshTokenValue,
            refreshTokenExpiry
        );
    }
}

