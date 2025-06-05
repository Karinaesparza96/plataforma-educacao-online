using FluentValidation;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Commands;

public class ValidarPagamentoCursoCommand : Command
{
    public Guid CursoId { get; set; }
    public Guid AlunoId { get; set; }
    public string NomeCartao { get; set; }
    public string NumeroCartao { get; set; }
    public string ExpiracaoCartao { get; set; }
    public string CvvCartao { get; set; }

    public ValidarPagamentoCursoCommand(Guid cursoId, Guid alunoId, string nomeCartao, 
                                        string numeroCartao, string expiracaoCartao, string cvvCartao)
    {
        AggregateId = alunoId;
        CursoId = cursoId;
        AlunoId = alunoId;
        NomeCartao = nomeCartao;
        NumeroCartao = numeroCartao;
        ExpiracaoCartao = expiracaoCartao;
        CvvCartao = cvvCartao;
    }
    public override bool EhValido()
    {
        ValidationResult = new ValidarPagamentoCursoCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ValidarPagamentoCursoCommandValidation : AbstractValidator<ValidarPagamentoCursoCommand>
{
    public static string CursoIdErro = "O campo CursoId é obrigatório.";
    public static string AlunoIdErro = "O campo AlunoId é obrigatório.";
    public static string NomeCartaoErro = "O campo Nome do Cartão é obrigatório.";
    public static string NumeroCartaoErro = "O campo Número do Cartão é obrigatório.";
    public static string ExpiracaoCartaoErro = "O campo Expiração do Cartão é obrigatório.";
    public static string CvvCartaoErro = "O campo CVV do Cartão é obrigatório.";
    public static string NumeroCartaoInvalido = "O campo Número do Cartão inválido.";

    public ValidarPagamentoCursoCommandValidation()
    {
        RuleFor(c => c.CursoId)
            .NotEqual(Guid.Empty)
            .WithMessage(CursoIdErro);
        RuleFor(c => c.AlunoId)
            .NotEqual(Guid.Empty)
            .WithMessage(AlunoIdErro);
        RuleFor(c => c.NomeCartao)
            .NotEmpty()
            .WithMessage(NomeCartaoErro);
        RuleFor(c => c.NumeroCartao)
            .NotEmpty()
            .WithMessage(NumeroCartaoErro)
            .CreditCard()
            .WithMessage(NumeroCartaoInvalido);
        RuleFor(c => c.ExpiracaoCartao)
            .NotEmpty()
            .WithMessage(ExpiracaoCartaoErro);
        RuleFor(c => c.CvvCartao)
            .NotEmpty()
            .WithMessage(CvvCartaoErro);
    }
}