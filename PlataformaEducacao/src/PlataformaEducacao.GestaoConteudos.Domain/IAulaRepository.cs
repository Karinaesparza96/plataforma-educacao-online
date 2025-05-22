using PlataformaEducacao.Core.Data;

namespace PlataformaEducacao.GestaoConteudos.Domain;

public interface IAulaRepository : IRepository<Aula>
{
    Task<ProgressoAula?> ObterProgressoAula(Guid aulaId, Guid alunoId);
    void AdicionarProgressoAula(ProgressoAula progressoAula);
}