using NetDevPack.SimpleMediator;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Handlers;

public class AulaCommandHandler(IAulaRepository aulaRepository, IMediator mediator) : IRequestHandler<AdicionarAulaCommand, bool>
{
    private readonly IAulaRepository _aulaRepository = aulaRepository;
    private readonly IMediator _mediator = mediator;
    public async Task<bool> Handle(AdicionarAulaCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request)) return false;

        var aula = new Aula(request.Nome, request.Conteudo);
        aula.AssociarCurso(request.CursoId);

        if (request is { NomeMaterial: not null, TipoMaterial: not null })
            aula.AdicionarMaterial(new Material(request.NomeMaterial, request.TipoMaterial));

        _aulaRepository.Adicionar(aula);

        return await _aulaRepository.UnitOfWork.Commit();
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