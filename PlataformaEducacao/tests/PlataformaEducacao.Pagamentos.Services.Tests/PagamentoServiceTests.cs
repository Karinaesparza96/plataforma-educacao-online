using MediatR;
using Moq;
using Moq.AutoMock;
using PlataformaEducacao.Core.DomainObjects.DTOs;
using PlataformaEducacao.Core.Messages.IntegrationCommands;
using PlataformaEducacao.Core.Messages.Notifications;

namespace PlataformaEducacao.Pagamentos.Business.Tests
{
    public class PagamentoServiceTests
    {
        private readonly AutoMocker _mocker;

        public PagamentoServiceTests()
        {
            _mocker = new AutoMocker();
        }

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

            var pagamentoService = _mocker.CreateInstance<PagamentoService>();
            _mocker.GetMock<IPagamentoService>().Setup(p => p.RealizarPagamentoCurso(It.IsAny<PagamentoCurso>()))
                .ReturnsAsync(true);
            _mocker.GetMock<IPagamentoCartaoCreditoFacade>()
                .Setup(p => p.RealizarPagamento(It.IsAny<Pedido>(), It.IsAny<Pagamento>()))
                .Returns(new Transacao { StatusTransacao = StatusTransacao.Pago });
            _mocker.GetMock<IPagamentoRepository>().Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            var result = await pagamentoService.RealizarPagamentoCurso(pagamentoCurso);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IPagamentoRepository>().Verify(r => r.Adicionar(It.IsAny<Pagamento>()));
            _mocker.GetMock<IPagamentoRepository>().Verify(r => r.AdicionarTransacao(It.IsAny<Transacao>()));
            _mocker.GetMock<IPagamentoRepository>().Verify(r => r.UnitOfWork.Commit());
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None),
                Times.Never);
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

            var pagamentoService = _mocker.CreateInstance<PagamentoService>();
            _mocker.GetMock<IPagamentoService>().Setup(p => p.RealizarPagamentoCurso(It.IsAny<PagamentoCurso>()))
                .ReturnsAsync(false);
            _mocker.GetMock<IPagamentoCartaoCreditoFacade>()
                .Setup(p => p.RealizarPagamento(It.IsAny<Pedido>(), It.IsAny<Pagamento>()))
                .Returns(new Transacao { StatusTransacao = StatusTransacao.Recusado });

            // Act
            var result = await pagamentoService.RealizarPagamentoCurso(pagamentoCurso);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IPagamentoRepository>().Verify(r => r.Adicionar(It.IsAny<Pagamento>()), Times.Never);
            _mocker.GetMock<IPagamentoRepository>()
                .Verify(r => r.AdicionarTransacao(It.IsAny<Transacao>()), Times.Never);
            _mocker.GetMock<IPagamentoRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Never);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<DomainNotification>(), CancellationToken.None),
                Times.Once);
        }

        [Fact(DisplayName = "Realizar Pagamento Command")]
        [Trait("Categoria", "Pagamentos - RealizarPagamentoCurso")]
        public void RealizarPagamentoCurso_Command_DeveExecutarComSucesso()
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
            var command = new RealizarPagamentoCursoCommand(pagamentoCurso);

            // Act
            var result = command.EhValido();

            // Assert
            Assert.True(result);

        }
    }
}