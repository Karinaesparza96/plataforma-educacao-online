using PlataformaEducacao.Pagamentos.Business;

namespace PlataformaEducacao.Pagamentos.AntiCorruption;

public interface IPayPalGateway
{
    string GetPayPalServiceKey(string apiKey, string encriptionKey);
    string GetCardHashKey(string serviceKey, string cartaoCredito);
    Transacao CommitTransaction(string cardHashKey, string orderId, decimal amount);
}