using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MediatR;
using PlataformaEducacao.Api.Controllers.Base;
using PlataformaEducacao.Api.DTOs;
using PlataformaEducacao.Api.Jwt;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

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

        var userIdentity = new IdentityUser()
        {
            UserName = registerUser.Nome,
            Email = registerUser.Email,
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(userIdentity, registerUser.Senha!);

        if (!result.Succeeded)
        {
            NotificarErro(result);
            return RespostaPadrao();
        }

        await userManager.AddToRoleAsync(userIdentity, "ALUNO");

        var command = new AdicionarAlunoCommand(userIdentity.Id, userIdentity.UserName);
        await _mediator.Send(command);

        var login = await signInManager.PasswordSignInAsync(userIdentity, registerUser.Senha!, false, true);

        if (!login.Succeeded)
        {
            NotificarErro(result);
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

        var userIdentity = new IdentityUser()
        {
            UserName = registerUser.Nome,
            Email = registerUser.Email,
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(userIdentity, registerUser.Senha!);

        if (!result.Succeeded)
        {
            NotificarErro(result);
            return RespostaPadrao();
        }

        await userManager.AddToRoleAsync(userIdentity, "ADMIN");

        var command = new AdicionarAdminCommand(userIdentity.Id);
        await _mediator.Send(command);

        var login = await signInManager.PasswordSignInAsync(userIdentity, registerUser.Senha!, false, true);

        if (!login.Succeeded)
        {
            NotificarErro(result);
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

        NotificarErro("Identity", "Usuário ou Senha incorretos");
        return RespostaPadrao();
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

        var encodedToken = tokenHandler.WriteToken(token);

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