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
using PlataformaEducacao.Core.Messages.Notifications;

namespace PlataformaEducacao.Api.Controllers;

[Route("api/conta")]
public class AutenticationController(INotificationHandler<DomainNotification> notificacoes,
                                IMediator mediator, 
                                SignInManager<IdentityUser> signInManager, 
                                UserManager<IdentityUser> userManager,
                                IOptions<JwtSettings> jwtSettings) : MainController(notificacoes, mediator)
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("registrar/aluno")]
    public async Task<ActionResult> RegistrarAluno(RegisterUserDto registerUser)
    {
        if (!ModelState.IsValid)
        {
            NotificarErro(ModelState);
            return RespostaPadrao();
        }

        var result = await RegistrarUsuario(registerUser, "ALUNO");

        if (!result.IdentityResult.Succeeded)
        {
            NotificarErro(result.IdentityResult);
            return RespostaPadrao();
        }

        var command = new AdicionarAlunoCommand(result.UsuarioId, registerUser.Nome);
        await _mediator.Send(command);

        if (!OperacaoValida())
            return RespostaPadrao();

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

        if (!result.IdentityResult.Succeeded)
        {
            NotificarErro(result.IdentityResult);
            return RespostaPadrao();
        }

        var command = new AdicionarAdminCommand(result.UsuarioId);
        await _mediator.Send(command);

        if (!OperacaoValida()) 
            return RespostaPadrao();

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
    private async Task<(IdentityResult IdentityResult, string UsuarioId)> RegistrarUsuario(RegisterUserDto registerUser, string role)
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

        return (result, result.Succeeded ? userIdentity.Id : string.Empty);
    }

    private async Task<LoginResponseDto> GerarToken(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        var roles = await userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var encodedToken = CodificarToken(claims);

        return ObterRespostaToken(user, claims, encodedToken);
    }

    private string CodificarToken(List<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(jwtSettings.Value.Segredo!);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = jwtSettings.Value.Emissor,
            Audience = jwtSettings.Value.Audiencia,
            Expires = DateTime.UtcNow.AddHours(jwtSettings.Value.ExpiracaoHoras),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        return tokenHandler.WriteToken(token);
    }

    private LoginResponseDto ObterRespostaToken(IdentityUser user, List<Claim> claims, string encodedToken)
    {
        var response = new LoginResponseDto
        {
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromHours(jwtSettings.Value.ExpiracaoHoras).TotalSeconds,
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