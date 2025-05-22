using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Commands;

public class AtivarMatriculaCommand(Guid alunoId, Guid cursoId) : Command
{
    public Guid AlunoId { get; set; } = alunoId;
    public Guid CursoId { get; set; } = cursoId;
    public override bool EhValido()
    {
        ValidationResult = new AtivarMatriculaCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class AtivarMatriculaCommandValidation : AbstractValidator<AtivarMatriculaCommand>
{
    public static string AlunoIdErro => "O campo AlunoId é obrigatório.";
    public static string CursoIdErro => "O campo CursoId é obrigatório.";
    public AtivarMatriculaCommandValidation()
    {
        RuleFor(c => c.AlunoId)
            .NotEqual(Guid.Empty)
            .WithMessage(AlunoIdErro);
        RuleFor(c => c.CursoId)
            .NotEqual(Guid.Empty)
            .WithMessage(CursoIdErro);
    }
}