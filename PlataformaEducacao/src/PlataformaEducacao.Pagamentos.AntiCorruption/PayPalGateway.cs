using PlataformaEducacao.Pagamentos.Business;

namespace PlataformaEducacao.Pagamentos.AntiCorruption;

public class PayPalGateway : IPayPalGateway
{
    public string GetPayPalServiceKey(string apiKey, string encriptionKey)
    {
        return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());
    }

    public string GetCardHashKey(string serviceKey, string cartaoCredito)
    {
        return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());
    }

    public Transacao CommitTransaction(string cardHashKey, string orderId, decimal amount)
    {
        var sucesso = true;
        return new Transacao
        {
            MatriculaId = Guid.Parse(orderId),
            Total = amount,
            StatusTransacao = sucesso ? StatusTransacao.Pago : StatusTransacao.Recusado
        };
    }
}