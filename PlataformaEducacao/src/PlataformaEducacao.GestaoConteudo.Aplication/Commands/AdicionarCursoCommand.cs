using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Commands;

public class AdicionarCursoCommand(string nome, string conteudoProgramatico, Guid usuarioCriacaoId, decimal preco) : Command
{
    public string Nome { get; set; } = nome;
    public string ConteudoProgramatico { get; set; } = conteudoProgramatico;
    public Guid UsuarioCriacaoId { get; set; } = usuarioCriacaoId;
    public decimal Preco { get; set; } = preco;

    public override bool EhValido()
    {
        ValidationResult = new AdicionarCursoCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class AdicionarCursoCommandValidation : AbstractValidator<AdicionarCursoCommand>
{
    public static string NomeErro => "O nome do curso não pode ser vazio.";
    public static string ConteudoProgramaticoErro => "O conteúdo programático não pode ser vazio.";
    public static string UsuarioCriacaoIdErro => "O ID do usuário de criação não pode ser vazio.";
    public static string PrecoErro => "O preço do curso deve ser maior que zero.";

    public AdicionarCursoCommandValidation()
    {
        RuleFor(c => c.Nome).NotEmpty().WithMessage(NomeErro);

        RuleFor(c => c.ConteudoProgramatico).NotEmpty().WithMessage(ConteudoProgramaticoErro);

        RuleFor(c => c.UsuarioCriacaoId).NotEmpty().WithMessage(UsuarioCriacaoIdErro);

        RuleFor(c => c.Preco)
            .GreaterThan(0)
            .WithMessage(PrecoErro);
    }
}