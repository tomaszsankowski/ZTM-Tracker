using MediatR;
using VueZtmBackend.Application.Auth.Commands;
using VueZtmBackend.Application.Common.Interfaces;
using VueZtmBackend.Domain.Entities;
using VueZtmBackend.Domain.Exceptions;
using VueZtmBackend.Domain.Interfaces;
using VueZtmBackend.Domain.ValueObjects;

namespace VueZtmBackend.Application.Auth.Handlers;

public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var login = new Login(request.Login);

            if (await _userRepository.ExistsAsync(login, cancellationToken))
            {
                return new RegisterResult(false, "Użytkownik o podanym loginie już istnieje.", null);
            }

            if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
            {
                return new RegisterResult(false, "Hasło musi mieć co najmniej 6 znaków.", null);
            }

            var hashedPassword = _passwordHasher.Hash(request.Password);
            var passwordHash = new PasswordHash(hashedPassword);

            var user = new User(login, passwordHash);

            await _userRepository.AddAsync(user, cancellationToken);

            return new RegisterResult(true, "Rejestracja zakończona pomyślnie.", user.Id);
        }
        catch (DomainException ex)
        {
            return new RegisterResult(false, ex.Message, null);
        }
    }
}

