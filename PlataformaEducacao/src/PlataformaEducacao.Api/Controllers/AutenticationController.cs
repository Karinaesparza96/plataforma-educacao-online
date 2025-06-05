using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MediatR;
using PlataformaEducacao.Api.Controllers.Base;
using PlataformaEducacao.Api.DTOs;
using PlataformaEducacao.Api.Jwt;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Core.Messages.Notifications;

namespace PlataformaEducacao.Api.Controllers;

[Route("api/conta")]
public class AutenticationController(INotificationHandler<DomainNotification> notificacoes,
                                IMediator mediator, 
                                SignInManager<IdentityUser> signInManager, 
                                UserManager<IdentityUser> userManager,
                                IAppIdentityUser identityUser,
                                IOptions<JwtSettings> jwtSettings) : MainController(notificacoes, mediator, identityUser)
{
    private readonly IMediator _mediator = mediator;
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    [HttpPost("registrar/aluno")]
    public async Task<ActionResult> RegistrarAluno(RegisterUserDto registerUser)
    {
        if (!ModelState.IsValid)
        {
            NotificarErro(ModelState);
            return RespostaPadrao();
        }

        var result = await RegistrarUsuario(registerUser, "ALUNO");

        if (!result.Result.Succeeded)
        {
            NotificarErro(result.Result);
            return RespostaPadrao();
        }

        var command = new AdicionarAlunoCommand(result.User.Id, registerUser.Nome);
        
        if (!await _mediator.Send(command))
        {
            await userManager.DeleteAsync(result.User);
            return RespostaPadrao();
        }

        var token = await GerarToken(registerUser.Email!);
        return RespostaPadrao(HttpStatusCode.Created, token);
    }
    [HttpPost("registrar/admin")]
    public async Task<ActionResult> RegistrarAdmin(RegisterUserDto registerUser)
    {
        if (!ModelState.IsValid)
        {
            NotificarErro(ModelState);
            return RespostaPadrao();
        }

        var result = await RegistrarUsuario(registerUser, "ADMIN");

        if (!result.Result.Succeeded)
        {
            NotificarErro(result.Result);
            return RespostaPadrao();
        }

        var command = new AdicionarAdminCommand(result.User.Id);

        if (!await _mediator.Send(command))
        {
            await userManager.DeleteAsync(result.User);
            return RespostaPadrao();
        }

        var token = await GerarToken(registerUser.Email!);
        return RespostaPadrao(HttpStatusCode.Created, token);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginUserDto loginUser)
    {
        if (!ModelState.IsValid)
        {
            NotificarErro(ModelState);
            return RespostaPadrao();
        }

        var result = await signInManager.PasswordSignInAsync(loginUser.Email!, loginUser.Senha!, false, true);

        if (result.Succeeded)
        {
            var loginResponse = await GerarToken(loginUser.Email!);
            return RespostaPadrao(HttpStatusCode.Created, loginResponse);
        }

        if (result.IsLockedOut)
        {
            NotificarErro("Identity", "Usuário bloqueado temporariamente. Tente novamente mais tarde.");
            return RespostaPadrao();
        }

        NotificarErro("Identity", "Usuário ou Senha incorretos");
        return RespostaPadrao();
    }
    private async Task<(IdentityUser User, IdentityResult Result)> RegistrarUsuario(RegisterUserDto registerUser, string role)
    {
        var userIdentity = new IdentityUser
        {
            UserName = registerUser.Nome,
            Email = registerUser.Email,
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(userIdentity, registerUser.Senha!);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(userIdentity, role);
        }

        return (userIdentity, result);
    }

    private async Task<LoginResponseDto> GerarToken(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        var roles = await userManager.GetRolesAsync(user);
        var claimsUser = await userManager.GetClaimsAsync(user);

        var identityClaims = GerarClaimsUser(claimsUser, roles, user);

        var encodedToken = CodificarToken(identityClaims);

        return ObterRespostaToken(user, claimsUser, encodedToken);
    }

    private ClaimsIdentity GerarClaimsUser(IList<Claim> claims, IList<string> roles, IdentityUser user)
    {
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.Now).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.Now).ToString(), ClaimValueTypes.Integer64));

        roles.ToList().ForEach(r =>
        {
            claims.Add(new Claim("role", r));
        });

        return new ClaimsIdentity(claims);
    }

    private static long ToUnixEpochDate(DateTime date)
    {
        return (long)Math.Round(
            (date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }

    private string CodificarToken(ClaimsIdentity claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_jwtSettings.Segredo!);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = claims,
            Issuer = _jwtSettings.Emissor,
            Audience = _jwtSettings.Audiencia,
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        return tokenHandler.WriteToken(token);
    }

    private LoginResponseDto ObterRespostaToken(IdentityUser user, IList<Claim> claims, string encodedToken)
    {
        var response = new LoginResponseDto
        {
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromHours(_jwtSettings.ExpiracaoHoras).TotalSeconds,
            UserToken = new UserTokenDto
            {
                Id = user.Id,
                Email = user.Email,
                Nome = user.UserName,
                Claims = claims.Select(c => new ClaimDto { Type = c.Type, Value = c.Value })
            }
        };

        return response;
    }
}