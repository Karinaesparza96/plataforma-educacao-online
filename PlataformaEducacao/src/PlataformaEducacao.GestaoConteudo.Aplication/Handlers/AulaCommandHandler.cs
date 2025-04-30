using NetDevPack.SimpleMediator;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Aplication.Handlers;

public class AulaCommandHandler(IMediator mediator, ICursoRepository cursoRepository) 
    : IRequestHandler<AdicionarAulaCommand, bool>, IRequestHandler<RealizarAulaCommand, bool>
{
    private readonly ICursoRepository _cursoRepository = cursoRepository;
    private readonly IMediator _mediator = mediator;
    public async Task<bool> Handle(AdicionarAulaCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request)) 
            return false;

        var curso = await _cursoRepository.ObterPorId(request.CursoId);

        if (curso == null)
        {
            await _mediator.Publish(new DomainNotification(request.MessageType, "Curso não encontrado."), cancellationToken);
            return false;
        }

        var aula = new Aula(request.Nome, request.Conteudo);
        aula.AssociarCurso(request.CursoId);

        if (request is { NomeMaterial: not null, TipoMaterial: not null })
            aula.AdicionarMaterial(new Material(request.NomeMaterial, request.TipoMaterial));

        _cursoRepository.Adicionar(aula);

        return await _cursoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(RealizarAulaCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var aula = await _cursoRepository.ObterAulaPorId(request.AulaId);

        if (aula == null)
        {
            await _mediator.Publish(new DomainNotification(request.MessageType, "Aula não encontrada."), cancellationToken);
            return false;
        }
        var progressoAula = new ProgressoAula(request.AlunoId, request.AulaId);

        aula.AdicionarProgresso(progressoAula);

        _cursoRepository.Adicionar(aula);

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