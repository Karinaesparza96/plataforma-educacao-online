using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Commands;

public class IniciarAulaCommand(Guid aulaId, Guid alunoId) : Command
{
    public Guid AulaId { get; set; } = aulaId;
    public Guid AlunoId { get; set; } = alunoId;

    public override bool EhValido()
    {
        ValidationResult = new IniciarAulaCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class IniciarAulaCommandValidation : AbstractValidator<IniciarAulaCommand>
{
    public static string AulaIdErro = "O campo AulaId é obrigatório.";
    public static string AlunoIdErro = "O campo AlunoId é obrigatório.";
    public IniciarAulaCommandValidation()
    {
        RuleFor(c => c.AulaId)
            .NotEqual(Guid.Empty)
            .WithMessage(AulaIdErro);
        RuleFor(c => c.AlunoId)
            .NotEqual(Guid.Empty)
            .WithMessage(AlunoIdErro);
    }
}