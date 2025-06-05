using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Commands;

public class AdicionarCertificadoCommand : Command
{
    public Guid AlunoId { get; set; }
    public Guid MatriculaId { get; set; }
    public Guid CursoId { get; set; }

    public AdicionarCertificadoCommand(Guid alunoId, Guid matriculaId, Guid cursoId)
    {
        AggregateId = alunoId;
        AlunoId = alunoId;
        MatriculaId = matriculaId;
        CursoId = cursoId;
    }
    public override bool EhValido()
    {
       ValidationResult = new GerarCertificadoCommandValidator().Validate(this);
       return ValidationResult.IsValid;
    }
}
public class GerarCertificadoCommandValidator : AbstractValidator<AdicionarCertificadoCommand>
{   
    public static string AlunoIdErro => "O campo AlunoId é obrigatório.";
    public static string MatriculaIdErro => "O campo MatriculaId é obrigatório.";
    public static string CursoIdErro => "O campo CursoId é obrigatório.";

    public GerarCertificadoCommandValidator()
    {
        RuleFor(c => c.AlunoId)
            .NotEqual(Guid.Empty)
            .WithMessage(AlunoIdErro);
        RuleFor(c => c.MatriculaId)
            .NotEqual(Guid.Empty)
            .WithMessage(MatriculaIdErro);
        RuleFor(c => c.CursoId)
            .NotEqual(Guid.Empty)
            .WithMessage(CursoIdErro);
    }
}