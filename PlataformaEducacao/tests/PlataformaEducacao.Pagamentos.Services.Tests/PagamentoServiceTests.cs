using MediatR;
using Moq;
using Moq.AutoMock;
using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Core.DomainObjects.DTOs;
using PlataformaEducacao.Core.Messages;

namespace PlataformaEducacao.Pagamentos.Business.Tests
{
    public class PagamentoServiceTests
    {
        [Fact(DisplayName = "Realizar Pagamento Curso Sucesso")]
        [Trait("Categoria", "Pagamentos - RealizarPagamentoCurso")]
        public async Task RealizarPagamentoCurso_StatusTransacaoPaga_DeveSalvarComSucesso()
        {
            // Arrange
            var pagamentoCurso = new PagamentoCurso
            {
                AlunoId = Guid.NewGuid(),
                CursoId = Guid.NewGuid(),
                NomeCartao = "Nome do Cartão",
                NumeroCartao = "5502093788528294",
                ExpiracaoCartao = "12/25",
                CvvCartao = "455",
                Total = 100.00m
            };
            var mocker = new AutoMocker();
            var pagamentoService = mocker.CreateInstance<PagamentoService>();
            mocker.GetMock<IPagamentoService>().Setup(p => p.RealizarPagamentoCurso(It.IsAny<PagamentoCurso>()))
                .ReturnsAsync(true);
            mocker.GetMock<IPagamentoCartaoCreditoFacade>().Setup(p => p.RealizarPagamento(It.IsAny<Pedido>(), It.IsAny<Pagamento>())).Returns(new Transacao { StatusTransacao = StatusTransacao.Pago });
            mocker.GetMock<IPagamentoRepository>().Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            var result = await pagamentoService.RealizarPagamentoCurso(pagamentoCurso);

            // Assert
            Assert.True(result);
            mocker.GetMock<IPagamentoRepository>().Verify(r => r.Adicionar(It.IsAny<Pagamento>()));
            mocker.GetMock<IPagamentoRepository>().Verify(r => r.AdicionarTransacao(It.IsAny<Transacao>()));
            mocker.GetMock<IPagamentoRepository>().Verify(r => r.UnitOfWork.Commit());
            mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Realizar Pagamento Curso Falha")]
        [Trait("Categoria", "Pagamentos - RealizarPagamentoCurso")]
        public async Task RealizarPagamentoCurso_StatusTransacaoRecusado_NãoDeveSalvar()
        {
            // Arrange
            var pagamentoCurso = new PagamentoCurso
            {
                AlunoId = Guid.NewGuid(),
                CursoId = Guid.NewGuid(),
                NomeCartao = "Nome do Cartão",
                NumeroCartao = "5502093788528294",
                ExpiracaoCartao = "12/25",
                CvvCartao = "455",
                Total = 100.00m
            };
            var mocker = new AutoMocker();
            var pagamentoService = mocker.CreateInstance<PagamentoService>();
            mocker.GetMock<IPagamentoService>().Setup(p => p.RealizarPagamentoCurso(It.IsAny<PagamentoCurso>()))
                .ReturnsAsync(false);
            mocker.GetMock<IPagamentoCartaoCreditoFacade>().Setup(p => p.RealizarPagamento(It.IsAny<Pedido>(), It.IsAny<Pagamento>())).Returns(new Transacao { StatusTransacao = StatusTransacao.Recusado });

            // Act
            var result = await pagamentoService.RealizarPagamentoCurso(pagamentoCurso);

            // Assert
            Assert.False(result);
            mocker.GetMock<IPagamentoRepository>().Verify(r => r.Adicionar(It.IsAny<Pagamento>()), Times.Never);
            mocker.GetMock<IPagamentoRepository>().Verify(r => r.AdicionarTransacao(It.IsAny<Transacao>()), Times.Never);
            mocker.GetMock<IPagamentoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
            mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None), Times.Once);
        }
    }
}