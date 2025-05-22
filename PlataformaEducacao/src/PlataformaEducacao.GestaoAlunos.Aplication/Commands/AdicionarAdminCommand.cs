using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Commands;

public class AdicionarAdminCommand(string usuarioId) : Command
{
    public string UsuarioId { get; set; } = usuarioId;

    public override bool EhValido()
    {
        ValidationResult = new AdicionarAdminCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class AdicionarAdminCommandValidation : AbstractValidator<AdicionarAdminCommand>
{
    public static string IdErro => "O campo UsuarioId deve ser informado";
    public AdicionarAdminCommandValidation()
    {
        RuleFor(c => c.UsuarioId)
            .NotEmpty()
            .WithMessage(IdErro);
    }
}