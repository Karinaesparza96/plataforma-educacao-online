using PlataformaEducacao.Core.Data;

namespace PlataformaEducacao.GestaoConteudos.Domain;

public interface IAulaRepository : IRepository<Aula>
{
    void Adicionar(Aula aula);
}