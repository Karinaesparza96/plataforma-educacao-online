using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Commands;

public class AtualizarCursoCommand(Guid cursoId, string nome, string conteudoProgramatico, decimal preco) : Command
{
    public Guid CursoId { get; set; } = cursoId;
    public string Nome { get; set; } = nome;
    public string ConteudoProgramatico { get; set; } = conteudoProgramatico;
    public decimal Preco { get; set; } = preco;

    public override bool EhValido()
    {
        ValidationResult = new AtualizarCursoCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}
public class AtualizarCursoCommandValidation : AbstractValidator<AtualizarCursoCommand>
{
    public static string CursoIdErro => "O ID do curso não pode ser vazio.";
    public static string NomeErro => "O nome do curso não pode ser vazio.";
    public static string ConteudoProgramaticoErro => "O conteúdo programático não pode ser vazio.";
    public static string PrecoErro => "O preço do curso deve ser maior que zero.";
    public AtualizarCursoCommandValidation()
    {
        RuleFor(c => c.CursoId).NotEmpty().WithMessage(CursoIdErro);
        RuleFor(c => c.Nome).NotEmpty().WithMessage(NomeErro);
        RuleFor(c => c.ConteudoProgramatico).NotEmpty().WithMessage(ConteudoProgramaticoErro);
        RuleFor(c => c.Preco)
            .GreaterThan(0)
            .WithMessage(PrecoErro);
    }
}