using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Commands;

public class AdicionarAlunoCommand : Command
{
    public string UsuarioId { get; set; }
    public string Nome { get; set; }

    public AdicionarAlunoCommand(string usuarioId, string nome)
    {
        if (Guid.TryParse(usuarioId, out var parsedGuid))
        {
            AggregateId = parsedGuid;
        }
        UsuarioId = usuarioId;
        Nome = nome;
    }

    public override bool EhValido()
    {
        ValidationResult = new AdicionarAlunoCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class AdicionarAlunoCommandValidation : AbstractValidator<AdicionarAlunoCommand>
{
    public static string IdErro => "O campo UsuarioId deve ser informado";
    public static string NomeErro => "O campo Nome deve ser informado";

    public AdicionarAlunoCommandValidation()
    {
        RuleFor(c => c.UsuarioId)
            .NotEmpty()
            .WithMessage(IdErro);
        RuleFor(c => c.Nome)
            .NotEmpty()
            .WithMessage(NomeErro);
    }
}
