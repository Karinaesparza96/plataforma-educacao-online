using MediatR;
using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Handlers;

public class UsuarioCommandHandler(IMediator mediator
                                   , IAlunoRepository alunoRepository
                                   , IUsuarioRepository usuarioRepository) : CommandHandler, IRequestHandler<AdicionarAlunoCommand, bool>, IRequestHandler<AdicionarAdminCommand, bool>
{
    public async Task<bool> Handle(AdicionarAlunoCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var aluno = new Aluno(request.Nome);
        aluno.AssociarUsuario(request.UsuarioId);

        alunoRepository.Adicionar(aluno);
        return await alunoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> Handle(AdicionarAdminCommand request, CancellationToken cancellationToken)
    {
        if (!ValidarComando(request))
            return false;

        var usuario = new Usuario();
        usuario.AssociarUsuario(request.UsuarioId);

        usuarioRepository.Adicionar(usuario);
        return await usuarioRepository.UnitOfWork.Commit();
    }

    protected override bool ValidarComando(Command command)
    {
        if (command.EhValido()) return true;
        foreach (var erro in command.ValidationResult.Errors)
        {
            mediator.Publish(new DomainNotification(command.MessageType, erro.ErrorMessage));
        }
        return false;
    }
}