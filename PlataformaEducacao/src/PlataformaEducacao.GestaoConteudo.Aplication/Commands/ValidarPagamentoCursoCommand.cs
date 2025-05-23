using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Commands;

public class ValidarPagamentoCursoCommand(Guid cursoId, Guid alunoId, string nomeCartao, string numeroCartao, string expiracaoCartao, string cvvCartao) : Command
{
    public Guid CursoId { get; set; } = cursoId;
    public Guid AlunoId { get; set; } = alunoId;
    public string NomeCartao { get; set; } = nomeCartao;
    public string NumeroCartao { get; set; } = numeroCartao;
    public string ExpiracaoCartao { get; set; } = expiracaoCartao;
    public string CvvCartao { get; set; } = cvvCartao;
    public override bool EhValido()
    {
        ValidationResult = new ValidarPagamentoCursoCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ValidarPagamentoCursoCommandValidation : AbstractValidator<ValidarPagamentoCursoCommand>
{
    public static string CursoId = "O campo CursoId é obrigatório.";
    public static string AlunoId = "O campo AlunoId é obrigatório.";
    public static string NomeCartao = "O campo Nome do Cartão é obrigatório.";
    public static string NumeroCartao = "O campo Número do Cartão é obrigatório.";
    public static string ExpiracaoCartao = "O campo Expiração do Cartão é obrigatório.";
    public static string CvvCartao = "O campo CVV do Cartão é obrigatório.";
    public static string NumeroCartaoInvalido = "O campo Número do Cartão inválido.";

    public ValidarPagamentoCursoCommandValidation()
    {
        RuleFor(c => c.CursoId)
            .NotEqual(Guid.Empty)
            .WithMessage(CursoId);
        RuleFor(c => c.AlunoId)
            .NotEqual(Guid.Empty)
            .WithMessage(AlunoId);
        RuleFor(c => c.NomeCartao)
            .NotEmpty()
            .WithMessage(NomeCartao);
        RuleFor(c => c.NumeroCartao)
            .NotEmpty()
            .WithMessage(NumeroCartao)
            .CreditCard()
            .WithMessage(NumeroCartaoInvalido);
        RuleFor(c => c.ExpiracaoCartao)
            .NotEmpty()
            .WithMessage(ExpiracaoCartao);
        RuleFor(c => c.CvvCartao)
            .NotEmpty()
            .WithMessage(CvvCartao);
    }
}