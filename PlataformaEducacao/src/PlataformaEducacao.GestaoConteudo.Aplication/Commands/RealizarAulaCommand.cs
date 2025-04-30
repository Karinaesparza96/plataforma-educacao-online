using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Commands;

public class RealizarAulaCommand(Guid aulaId, Guid alunoId, Guid cursoId) : Command
{
    public Guid AulaId { get; set; } = aulaId;
    public Guid AlunoId { get; set; } = alunoId;
    public Guid CursoId { get; set; } = cursoId;

    public override bool EhValido()
    {
        ValidationResult = new RealizarAulaCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class RealizarAulaCommandValidation : AbstractValidator<RealizarAulaCommand>
{
    public static string AulaIdErro = "O campo AulaId é obrigatório.";
    public static string AlunoIdErro = "O campo AlunoId é obrigatório.";
    public static string CursoIdErro = "O campo CursoId é obrigatório.";
    public RealizarAulaCommandValidation()
    {
        RuleFor(c => c.AulaId)
            .NotEqual(Guid.Empty)
            .WithMessage(AulaIdErro);
        RuleFor(c => c.AlunoId)
            .NotEqual(Guid.Empty)
            .WithMessage(AlunoIdErro);
        RuleFor(c => c.CursoId)
            .NotEqual(Guid.Empty)
            .WithMessage(CursoIdErro);
    }
}