using PlataformaEducacao.Core.Data;

namespace PlataformaEducacao.GestaoAlunos.Domain;

public interface IStatusMatriculaRepository : IRepository<StatusMatricula>
{
    Task<StatusMatricula?> ObterPorCodigo(int codigo);
}