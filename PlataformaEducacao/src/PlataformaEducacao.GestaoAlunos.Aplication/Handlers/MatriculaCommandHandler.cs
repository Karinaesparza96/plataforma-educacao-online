using NetDevPack.SimpleMediator;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Handlers;

public class MatriculaCommandHandler(IMediator mediator, IAlunoRepository alunoRepository) : CommandHandler, IRequestHandler<CriarMatriculaCommand, bool>
{
    private readonly IMediator _mediator = mediator;
    private readonly IAlunoRepository _alunoRepository = alunoRepository;

    public async Task<bool> Handle(CriarMatriculaCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var aluno = await _alunoRepository.ObterPorId(request.AlunoId);

        if (aluno == null)
        {
            await _mediator.Publish(new DomainNotification(request.MessageType, "Aluno não encontrado."), cancellationToken);
            return false;
        }
        
        var matricula = new Matricula(request.AlunoId, request.CursoId);
        aluno.AdicionarMatricula(matricula);

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