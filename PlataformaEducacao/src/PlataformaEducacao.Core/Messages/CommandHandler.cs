namespace PlataformaEducacao.Core.Messages;

public abstract class CommandHandler
{
    protected abstract bool ValidarComando(Command command);
}