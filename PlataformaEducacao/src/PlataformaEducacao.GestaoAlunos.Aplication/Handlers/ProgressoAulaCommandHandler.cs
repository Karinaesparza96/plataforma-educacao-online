using NetDevPack.SimpleMediator;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.Core.Messages.Queries;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Handlers;

public class ProgressoAulaCommandHandler(IMediator mediator, IAlunoRepository alunoRepository) : CommandHandler, IRequestHandler<IniciarAulaCommand, bool>
{
    private readonly IMediator _mediator = mediator;
    private readonly IAlunoRepository _alunoRepository = alunoRepository;
    public async Task<bool> Handle(IniciarAulaCommand request, CancellationToken cancellationToken)
    {
        if(!ValidarComando(request)) return false;

        var aluno = await _alunoRepository.ObterPorId(request.AlunoId);

        if (aluno == null)
        {
            await _mediator.Publish(new DomainNotification(request.MessageType, "Aluno não encontrado."), cancellationToken);
            return false;
        }

        var cursoDto = await _mediator.Send(new ObterCursoPorAulaIdQuery(request.AulaId), cancellationToken); 

        if (cursoDto == null)
        {
            await _mediator.Publish(new DomainNotification(request.MessageType, "Aula não pertence a nenhum curso"), cancellationToken);
            return false;
        }

        var matricula = await _alunoRepository.ObterMatriculaCursoPorAlunoId(request.AlunoId, cursoDto.Id);

        if (matricula == null)
        {
            await _mediator.Publish(new DomainNotification(request.MessageType, "Aluno não está matriculado no curso"), cancellationToken);
            return false;
        }

        var progressoAula = new ProgressoAula(request.AulaId, request.AlunoId);
        aluno.AdicionarProgressoAula(progressoAula);

        return await _alunoRepository.UnitOfWork.Commit();
    }


    protected override bool ValidarComando(Command command)
    {
        if (command.EhValido()) return true;
        foreach (var erro in command.ValidationResult.Errors)
        {
            _mediator.Publish(new DomainNotification(command.MessageType, erro.ErrorMessage));
        }
        return false;
    }
}