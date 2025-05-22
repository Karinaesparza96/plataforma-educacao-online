using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Commands;

public class DeletarCursoCommand(Guid cursoId) : Command
{
    public Guid CursoId { get; set; } = cursoId;
    public override bool EhValido()
    {
        ValidationResult = new DeletarCursoCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class DeletarCursoCommandValidation : AbstractValidator<DeletarCursoCommand>
{
    public static string CursoIdErro => "O ID do curso não pode ser vazio.";
    public DeletarCursoCommandValidation()
    {
        RuleFor(c => c.CursoId).NotEmpty().WithMessage(CursoIdErro);
    }
}