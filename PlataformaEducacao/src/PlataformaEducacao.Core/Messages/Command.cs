using FluentValidation.Results;
using MediatR;

namespace PlataformaEducacao.Core.Messages;

public abstract class Command : Message, IRequest<bool>
{
    public DateTime Timestamp { get; protected set; }
    public ValidationResult ValidationResult { get; set; }
    protected Command()
    {
        Timestamp = DateTime.Now;
    }
    public abstract bool EhValido();
}