using NetDevPack.SimpleMediator;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Handlers;

public class CursoCommandHandler(ICursoRepository cursoRepository, IMediator mediator) : IRequestHandler<AdicionarCursoCommand, bool>
{   
    private readonly ICursoRepository _cursoRepository = cursoRepository;
    private readonly IMediator _mediator = mediator;

    public async Task<bool> Handle(AdicionarCursoCommand command, CancellationToken cancellationToken)
    {
        if (!ValidarComando(command)) return false;

        var curso = new Curso(command.Nome, command.ConteudoProgramatico, command.UsuarioCriacaoId);
        _cursoRepository.Adicionar(curso);

        return await _cursoRepository.UnitOfWork.Commit();
    }

    private bool ValidarComando(Command command)
    {
        if (command.EhValido()) return true;

        foreach (var erro in command.ValidationResult.Errors)
        {
            _mediator.Publish(new DomainNotification(command.MessageType, erro.ErrorMessage));
        }
        return false;
    }
}