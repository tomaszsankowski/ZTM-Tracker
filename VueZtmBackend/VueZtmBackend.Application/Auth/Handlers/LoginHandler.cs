using MediatR;
using VueZtmBackend.Application.Auth.Commands;
using VueZtmBackend.Application.Common.Interfaces;
using VueZtmBackend.Domain.Exceptions;
using VueZtmBackend.Domain.Interfaces;
using VueZtmBackend.Domain.ValueObjects;

namespace VueZtmBackend.Application.Auth.Handlers;

public class LoginHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public LoginHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var login = new Login(request.Login);
            var user = await _userRepository.GetByLoginAsync(login, cancellationToken);

            if (user is null)
            {
                return new LoginResult(false, "Nieprawidłowy login lub hasło.", null, null, null);
            }

            if (!_passwordHasher.Verify(request.Password, user.PasswordHash.Value))
            {
                return new LoginResult(false, "Nieprawidłowy login lub hasło.", null, null, null);
            }

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshTokenValue = _jwtService.GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            var refreshToken = RefreshToken.Create(refreshTokenValue, refreshTokenExpiry);
            user.UpdateRefreshToken(refreshToken);
            await _userRepository.UpdateAsync(user, cancellationToken);

            return new LoginResult(
                true,
                "Logowanie pomyślne.",
                accessToken,
                refreshTokenValue,
                refreshTokenExpiry
            );
        }
        catch (DomainException ex)
        {
            return new LoginResult(false, ex.Message, null, null, null);
        }
    }
}

