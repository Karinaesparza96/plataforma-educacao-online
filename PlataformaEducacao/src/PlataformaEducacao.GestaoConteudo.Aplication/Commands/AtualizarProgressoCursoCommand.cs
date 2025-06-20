using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Commands;

public class AtualizarProgressoCursoCommand : Command
{
    public Guid CursoId { get; set; }
    public Guid AlunoId { get; set; }

    public AtualizarProgressoCursoCommand(Guid cursoId, Guid alunoId)
    {
        AggregateId = cursoId;
        CursoId = cursoId;
        AlunoId = alunoId;
    }
    public override bool EhValido()
    {
        ValidationResult = new AtualizarProgressoCursoCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class AtualizarProgressoCursoCommandValidation : AbstractValidator<AtualizarProgressoCursoCommand>
{
    static string CursoIdErro = "O campo CursoId é obrigatório.";
    static string AlunoIdErro = "O campo AlunoId é obrigatório.";
    public AtualizarProgressoCursoCommandValidation()
    {
        RuleFor(c => c.CursoId)
            .NotEqual(Guid.Empty)
            .WithMessage(CursoIdErro);
        RuleFor(c => c.AlunoId)
            .NotEqual(Guid.Empty)
            .WithMessage(AlunoIdErro);
    }
}