using System.Linq;

namespace PlataformaEducacao.GestaoAlunos.Domain.Tests
{
    public class ProgressoAulaTests
    {
        [Fact(DisplayName = "Aluno - Iniciar Aula")]
        [Trait("Categoria", "GestaoAlunos - ProgressoAulas")]
        public void AdicionarProgressoAula_AulaNaoAssistida_DeveRegistrarProgressoEmAndamento()
        {
            // Arrange
            var progressoAula = new ProgressoAula(Guid.NewGuid(), Guid.NewGuid());
            var aluno = new Aluno();
            aluno.AdicionarProgressoAula(progressoAula);

            // Act & Assert
            Assert.Equal(EProgressoAulaStatus.EmAndamento, progressoAula.Status);
            Assert.True(aluno.ProgressoAulas.Any(x => x.Id == progressoAula.Id));
        }

        [Fact(DisplayName = "Aluno - Concluir Aula")]
        [Trait("Categoria", "GestaoAlunos - ProgressoAulas")]
        public void AdicionarProgressoAula_AulaEmAndamento_DeveRegistrarProgressConcluida()
        {
            // Arrange
            var progressoAula = new ProgressoAula(Guid.NewGuid(), Guid.NewGuid());
            var aluno = new Aluno();
            aluno.AdicionarProgressoAula(progressoAula);
            // Act
            progressoAula.ConcluirAula();

            // Assert
            Assert.Equal(EProgressoAulaStatus.Concluida, progressoAula.Status);
            Assert.True(aluno.ProgressoAulas.Any(x => x.Id == progressoAula.Id));
        }
    }
}