using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Commands;

public class AdicionarMatriculaCommand : Command
{
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }

    public AdicionarMatriculaCommand(Guid alunoId, Guid cursoId)
    {
        AggregateId = alunoId;
        AlunoId = alunoId;
        CursoId = cursoId;
    }

    public override bool EhValido()
    {
        ValidationResult = new AdicionarMatriculaCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class AdicionarMatriculaCommandValidation : AbstractValidator<AdicionarMatriculaCommand>
{
    public static string AlunoIdErro = "O campo AlunoId não pode ser vazio.";
    public static string CursoIdErro = "O campo CursoId não pode ser vazio.";
    public AdicionarMatriculaCommandValidation()
    {
        RuleFor(c => c.AlunoId)
            .NotEqual(Guid.Empty)
            .WithMessage(AlunoIdErro);
        RuleFor(c => c.CursoId)
            .NotEqual(Guid.Empty)
            .WithMessage(CursoIdErro);
    }
}