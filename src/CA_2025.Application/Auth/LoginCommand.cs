using CA_2025.Application.Services;
using CA_2025.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace CA_2025.Application.Auth;

public sealed record LoginCommand(
    string UserNameOrEmail,
    string Password) : IRequest<Result<LoginCommandResponse>>;

public sealed record LoginCommandResponse()
{
    public string AccessToken { get; set; } = default!;
}

internal sealed class LoginCommandHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == request.UserNameOrEmail || x.UserName == request.UserNameOrEmail, cancellationToken);

        if (user is null)
        {
            return Result<LoginCommandResponse>.Failure("Kullanıcı bulunamadı.");
        }

        if (user.IsDeleted)
        {
            return Result<LoginCommandResponse>.Failure("Kullanıcı Pasif durumdadır.");
        }

        SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);
        if (signInResult.IsLockedOut)
        {
            TimeSpan? timeSpan = user.LockoutEnd - DateTime.UtcNow;
            if (timeSpan is not null)
            {
                return Result<LoginCommandResponse>.Failure($"Şifrenizi 5 defa yanlış girdiğiniz için kullanıcı {Math.Ceiling(timeSpan.Value.TotalMinutes)} dakika süreyle bloke edilmiştir.");
            }
            else
            {
                return Result<LoginCommandResponse>.Failure("Kullanıcınız 5 kez yanlış şifre ");
            }
        }

        if (signInResult.IsNotAllowed)
        {
            return Result<LoginCommandResponse>.Failure("Mail adresiniz onaylı değil");
        }

        if (!signInResult.Succeeded)
        {
            return Result<LoginCommandResponse>.Failure("Şifreniz yanlış");
        }

        string token = await jwtProvider.CreateTokenAsync(user, cancellationToken);
        //token üret
        var response = new LoginCommandResponse() { AccessToken = token };

        return response;
    }
}