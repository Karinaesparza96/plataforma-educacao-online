using PlataformaEducacao.Core.Data;

namespace PlataformaEducacao.GestaoAlunos.Domain;

public interface IUsuarioRepository : IRepository<Usuario>
{
    void Adicionar(Usuario usuario);
}