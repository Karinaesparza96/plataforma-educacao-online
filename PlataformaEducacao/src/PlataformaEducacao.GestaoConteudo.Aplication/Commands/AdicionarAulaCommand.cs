using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Commands;

public class AdicionarAulaCommand : Command
{
    public string Nome { get; set; }
    public string Conteudo { get; set; }
    public Guid CursoId { get; set; }
    public string? NomeMaterial { get; set; }
    public string? TipoMaterial { get; set; }

    public AdicionarAulaCommand(string nome, string conteudo, Guid cursoId,
                                string nomeMaterial, string tipoMaterial)
    {
        AggregateId = cursoId;
        Nome = nome;
        Conteudo = conteudo;
        CursoId = cursoId;
        NomeMaterial = nomeMaterial;
        TipoMaterial = tipoMaterial;
    }

    public override bool EhValido()
    {
        ValidationResult = new AdicionarAulaCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class AdicionarAulaCommandValidation : AbstractValidator<AdicionarAulaCommand>
{
    public static string NomeErro = "O campo Nome não pode ser vazio.";
    public static string ConteudoErro = "O campo Conteudo não pode ser vazio.";
    public static string CursoIdErro = "O campo CursoId é obrigatório.";
    public AdicionarAulaCommandValidation()
    {
        RuleFor(c => c.Nome)
            .NotEmpty()
            .WithMessage(NomeErro);
        RuleFor(c => c.Conteudo)
            .NotEmpty()
            .WithMessage(ConteudoErro);
        RuleFor(c => c.CursoId)
            .NotEqual(Guid.Empty)
            .WithMessage(CursoIdErro);
    }
}