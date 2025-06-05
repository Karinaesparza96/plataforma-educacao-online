namespace PlataformaEducacao.Core.Messages;

public abstract class CommandHandler
{
    protected virtual Task AdicionarNotificacao(string type, string descricao, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    protected virtual void AdicionarNotificacao(string type, string descricao)
    {
        throw new NotImplementedException();
    }
}