using MediatR;
using PlataformaEducacao.Core.Messages.IntegrationCommands;

namespace PlataformaEducacao.Pagamentos.Business.Handlers;

public class PagamentoCommandHandler(IPagamentoService pagamentoService) : IRequestHandler<RealizarPagamentoCursoCommand, bool>
{
    public async Task<bool> Handle(RealizarPagamentoCursoCommand request, CancellationToken cancellationToken)
    {
        return await pagamentoService.RealizarPagamentoCurso(request.PagamentoCurso);
    }
}