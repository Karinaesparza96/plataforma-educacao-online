using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Commands;

public class AdicionarCertificadoCommand(Guid alunoId, Guid matriculaId, Guid cursoId) : Command
{
    public Guid AlunoId { get; set; } = alunoId;
    public Guid MatriculaId { get; set; } = matriculaId;
    public Guid CursoId { get; set; } = cursoId;
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