using System.ComponentModel.DataAnnotations;

namespace PlataformaEducacao.Api.DTOs;

public class RegisterUserDto
{
    [Required(ErrorMessage = "Campo {0} é obrigatório.")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "Campo {0} é obrigatório.")]
    [EmailAddress(ErrorMessage = "Campo {0} está em formato inválido.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Campo {0} é obrigatório.")]
    [StringLength(100, ErrorMessage = "Campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 8)]
    public string? Senha { get; set; }

    [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
    public string? ConfirmacaoSenha { get; set; }
}
