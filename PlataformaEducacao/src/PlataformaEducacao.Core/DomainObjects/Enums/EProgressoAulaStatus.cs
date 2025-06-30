using System.ComponentModel;
namespace PlataformaEducacao.Core.DomainObjects.Enums;

public enum EProgressoAulaStatus
{
    [Description("Não Iniciada")]
    NaoIniciada = 0,
    [Description("Em Andamento")]
    EmAndamento = 1,
    [Description("Concluída")]
    Concluida = 2,
}