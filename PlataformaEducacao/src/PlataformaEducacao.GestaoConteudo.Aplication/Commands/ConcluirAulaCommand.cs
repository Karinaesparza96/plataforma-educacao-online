using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Commands;

public class ConcluirAulaCommand : Command
{
    public Guid AulaId { get; set; }
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }

    public ConcluirAulaCommand(Guid aulaId, Guid alunoId, Guid cursoId)
    {
        AggregateId = cursoId;
        AulaId = aulaId;
        AlunoId = alunoId;
        CursoId = cursoId;
    }
    public override bool EhValido()
    {
        ValidationResult = new ConcluirAulaCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ConcluirAulaCommandValidation : AbstractValidator<ConcluirAulaCommand>
{
    public static string AulaIdErro = "O campo AulaId é obrigatório.";
    public static string AlunoIdErro = "O campo AlunoId é obrigatório.";
    public static string CursoIdErro = "O campo CursoId é obrigatório.";
    public ConcluirAulaCommandValidation()
    {
        RuleFor(c => c.AulaId)
            .NotEqual(Guid.Empty)
            .WithMessage(AulaIdErro);
        RuleFor(c => c.AlunoId)
            .NotEmpty()
            .WithMessage(AlunoIdErro);
        RuleFor(c => c.CursoId)
            .NotEqual(Guid.Empty)
            .WithMessage(CursoIdErro);
    }
}