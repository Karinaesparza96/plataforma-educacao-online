﻿namespace PlataformaEducacao.Pagamentos.Business;

public interface IPagamentoCartaoCreditoFacade
{
    Transacao RealizarPagamento(Pedido pedido, Pagamento pagamento);
}