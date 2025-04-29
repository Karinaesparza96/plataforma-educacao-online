using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Commands;

public class CriarMatriculaCommand : Command
{
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }

    public CriarMatriculaCommand(Guid alunoId, Guid cursoId)
    {
        AlunoId = alunoId;
        CursoId = cursoId;
    }

    public override bool EhValido()
    {
        ValidationResult = new CriarMatriculaCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class CriarMatriculaCommandValidation : AbstractValidator<CriarMatriculaCommand>
{
    public static string AlunoIdErro = "O campo AlunoId não pode ser vazio.";
    public static string CursoIdErro = "O campo CursoId não pode ser vazio.";
    public CriarMatriculaCommandValidation()
    {
        RuleFor(c => c.AlunoId)
            .NotEqual(Guid.Empty)
            .WithMessage(AlunoIdErro);
        RuleFor(c => c.CursoId)
            .NotEqual(Guid.Empty)
            .WithMessage(CursoIdErro);
    }
}