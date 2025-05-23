using FluentValidation;
using PlataformaEducacao.Core.DomainObjects.DTOs;

namespace PlataformaEducacao.Core.Messages.IntegrationCommands;

public class RealizarPagamentoCursoCommand(PagamentoCurso pagamentoCurso) : Command
{
    public PagamentoCurso PagamentoCurso { get; set; } = pagamentoCurso;
    public override bool EhValido()
    {
        ValidationResult = new RealizarPagamentoCursoCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class RealizarPagamentoCursoCommandValidation : AbstractValidator<RealizarPagamentoCursoCommand>
{
    public static string PagamentoCursoErro => "O pagamento do curso não pode ser vazio.";
    public RealizarPagamentoCursoCommandValidation()
    {
        RuleFor(c => c.PagamentoCurso).NotEmpty().WithMessage(PagamentoCursoErro);
    }
}