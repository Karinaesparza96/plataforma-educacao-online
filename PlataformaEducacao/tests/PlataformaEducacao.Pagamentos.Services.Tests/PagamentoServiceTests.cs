using MediatR;
using Moq;
using Moq.AutoMock;
using PlataformaEducacao.Core.DomainObjects.DTOs;
using PlataformaEducacao.Core.Messages.Notifications;
using PlataformaEducacao.Pagamentos.Business.Commands;
using PlataformaEducacao.Pagamentos.Business.Handlers;

namespace PlataformaEducacao.Pagamentos.Business.Tests
{
    public class PagamentoServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly PagamentoCommandHandler _handler;

        public PagamentoServiceTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<PagamentoCommandHandler>();
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

        [Fact(DisplayName = "Realizar Pagamento Command Valido")]
        [Trait("Categoria", "Pagamentos - RealizarPagamentoCurso")]
        public void RealizarPagamentoCurso_Command_DeveExecutarComSucesso()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var cursoPreco = 150;
            var alunoId = Guid.NewGuid();
            var nomeCartao = "Nome do Cartão";
            var numeroCartao = "5502093788528294";
            var expiracaoCartao = "12/25";
            var cvvCartao = "455";

            var command = new RealizarPagamentoCursoCommand(alunoId, cursoId, cvvCartao, expiracaoCartao, nomeCartao,
                numeroCartao, cursoPreco);

            // Act
            var result = command.EhValido();

            // Assert
            Assert.True(result);
        }
        [Fact(DisplayName = "Realizar Pagamento Command Invalido")]
        [Trait("Categoria", "Pagamentos - RealizarPagamentoCurso")]
        public void RealizarPagamentoCurso_CommandInvalido_DeveRetornarErros()
        {
            // Arrange
            var cursoId = Guid.Empty;
            var cursoPreco = 0;
            var alunoId = Guid.Empty;
            var nomeCartao = "";
            var numeroCartao = "1234567891";
            var expiracaoCartao = "";
            var cvvCartao = "";

            var command = new RealizarPagamentoCursoCommand(alunoId, cursoId, cvvCartao, expiracaoCartao, nomeCartao,
                numeroCartao, cursoPreco);

            // Act
            var result = command.EhValido();

            // Assert
            Assert.False(result);
            Assert.Contains(RealizarPagamentoCursoCommandValidation.AlunoIdErro,
                command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(RealizarPagamentoCursoCommandValidation.CursoIdErro,
                command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(RealizarPagamentoCursoCommandValidation.NomeCartaoErro,
                command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(RealizarPagamentoCursoCommandValidation.NumeroCartaoInvalidoErro,
                command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(RealizarPagamentoCursoCommandValidation.ExpiracaoCartaoErro,
                command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(RealizarPagamentoCursoCommandValidation.CvvCartaoErro,
                command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(RealizarPagamentoCursoCommandValidation.TotalErro,
                command.ValidationResult.Errors.Select(e => e.ErrorMessage));
            Assert.Equal(7, command.ValidationResult.Errors.Count);
        }

        [Fact(DisplayName = "Realizar Pagamento CommandHandler Sucesso")]
        [Trait("Categoria", "Pagamentos - RealizarPagamentoCurso")]
        public async Task RealizarPagamentoCurso_DadosValidos_DeveExecutarComSucesso()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var cursoPreco = 150;
            var alunoId = Guid.NewGuid();
            var nomeCartao = "Nome do Cartão";
            var numeroCartao = "5502093788528294";
            var expiracaoCartao = "12/25";
            var cvvCartao = "455";

            var command = new RealizarPagamentoCursoCommand(alunoId, cursoId, cvvCartao, expiracaoCartao, nomeCartao,
                numeroCartao, cursoPreco);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mocker.GetMock<IPagamentoService>()
                .Verify(p => p.RealizarPagamentoCurso(It.IsAny<PagamentoCurso>()), Times.Once);
        }


        [Fact(DisplayName = "Realizar Pagamento CommandHandler Falha")]
        [Trait("Categoria", "Pagamentos - RealizarPagamentoCurso")]
        public async Task RealizarPagamentoCurso_DadosInvalidos_NaoDeveExecutarComSucesso()
        {
            // Arrange
            var cursoId = Guid.Empty;
            var cursoPreco = 0;
            var alunoId = Guid.Empty;
            var nomeCartao = "";
            var numeroCartao = "1234567891";
            var expiracaoCartao = "";
            var cvvCartao = "";

            var command = new RealizarPagamentoCursoCommand(alunoId, cursoId, cvvCartao, expiracaoCartao, nomeCartao,
                numeroCartao, cursoPreco);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mocker.GetMock<IPagamentoService>()
                .Verify(p => p.RealizarPagamentoCurso(It.IsAny<PagamentoCurso>()), Times.Never);
        }
    }
}