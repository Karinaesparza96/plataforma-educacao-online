namespace PlataformaEducacao.Core.DomainObjects.Enums;

public enum EStatusMatricula : int
{
    Iniciada,
    AguardandoPagamento = 1,
    Ativa = 2,
    Concluida = 3,
    Cancelada = 4,
}