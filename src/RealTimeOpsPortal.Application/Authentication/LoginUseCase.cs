namespace RealTimeOpsPortal.Application.Authentication;

public class LoginUseCase
{
    private readonly IUserAuthenticationRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginUseCase(
        IUserAuthenticationRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<LoginResult?> ExecuteAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken);

        if (user is null || !user.IsActive)
            return null;

        var passwordHash =
            await _userRepository.GetPasswordHashAsync(
                user.Id,
                cancellationToken);

        if (passwordHash is null)
            return null;

        var isValid = _passwordHasher.Verify(
            request.Password,
            passwordHash);

        if (!isValid)
            return null;

        var token = _tokenService.GenerateToken(user);

        return new LoginResult(
            token,
            user.Id,
            user.Email,
            user.DisplayName,
            user.Role.ToString());
    }
}