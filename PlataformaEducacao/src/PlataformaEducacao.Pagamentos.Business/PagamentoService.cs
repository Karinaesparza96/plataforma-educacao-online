using MediatR;
using PlataformaEducacao.Core.DomainObjects.DTOs;
using PlataformaEducacao.Core.Messages.IntegrationEvents;
using PlataformaEducacao.Core.Messages.Notifications;

namespace PlataformaEducacao.Pagamentos.Business;

public class PagamentoService(IPagamentoCartaoCreditoFacade pagamentoCartaoCreditoFacade,
                              IPagamentoRepository pagamentoRepository,
                              IMediator mediator) : IPagamentoService
{
    public async Task<bool> RealizarPagamentoCurso(PagamentoCurso pagamentoCurso)
    {
        var pedido = new Pedido
        {
            CursoId = pagamentoCurso.CursoId,
            AlunoId = pagamentoCurso.AlunoId,
            Valor = pagamentoCurso.Total,
        };

        var pagamento = new Pagamento
        {
            Valor = pagamentoCurso.Total,
            NomeCartao = pagamentoCurso.NomeCartao,
            NumeroCartao = pagamentoCurso.NumeroCartao,
            ExpiracaoCartao = pagamentoCurso.ExpiracaoCartao,
            CvvCartao = pagamentoCurso.CvvCartao,
            AlunoId = pagamentoCurso.AlunoId,
            CursoId = pagamentoCurso.CursoId
        };

        var transacao = pagamentoCartaoCreditoFacade.RealizarPagamento(pedido, pagamento);

        if (transacao.StatusTransacao == StatusTransacao.Pago)
        {
            pagamento.AdicionarEvento(new CursoPagamentoRealizadoEvent(pagamento.CursoId, pagamento.AlunoId));

            pagamentoRepository.Adicionar(pagamento);
            pagamentoRepository.AdicionarTransacao(transacao);

            await pagamentoRepository.UnitOfWork.Commit();
            return true;
        }

        await mediator.Publish(new DomainNotification("pagamento", "A operadora recusou o pagamento"));
        return false;
    }
}