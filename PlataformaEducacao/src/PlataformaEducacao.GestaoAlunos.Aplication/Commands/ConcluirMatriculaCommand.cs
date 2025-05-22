using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Commands;

public class ConcluirMatriculaCommand : Command
{
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }

    public ConcluirMatriculaCommand(Guid alunoId, Guid cursoId)
    {
        AlunoId = alunoId;
        CursoId = cursoId;
    }
    public override bool EhValido()
    {
        ValidationResult = new ConcluirMatriculaCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ConcluirMatriculaCommandValidation : AbstractValidator<ConcluirMatriculaCommand>
{
    public static string AlunoId = "O campo AlunoId é obrigatório.";
    public static string CursoId = "O campo CursoId é obrigatório.";
    public ConcluirMatriculaCommandValidation()
    {
        RuleFor(c => c.AlunoId)
            .NotEqual(Guid.Empty)
            .WithMessage(AlunoId);
        RuleFor(c => c.CursoId)
            .NotEqual(Guid.Empty)
            .WithMessage(CursoId);
    }
}