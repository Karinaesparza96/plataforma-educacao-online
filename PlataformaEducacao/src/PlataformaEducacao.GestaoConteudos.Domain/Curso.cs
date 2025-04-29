using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Core.DomainObjects.DTOs;

namespace PlataformaEducacao.GestaoConteudos.Domain;
public class Curso : Entity, IAggregateRoot
{
    public string Nome { get; private set; }
    public string ConteudoProgramatico { get; private set; }
    public Guid UsuarioCriacaoId { get; private set; }

    private readonly List<Aula> _aulas;
    public IReadOnlyCollection<Aula> Aulas => _aulas;

    public Curso(string nome, string conteudoProgramatico, Guid usuarioCriacaoId)
    {
        Nome = nome;
        ConteudoProgramatico = conteudoProgramatico;
        UsuarioCriacaoId = usuarioCriacaoId;
        _aulas = new List<Aula>();
    }

    public void AdicionarAula(Aula aula)
    {
        if (AulaExistente(aula))
            throw new DomainException("Aula já associada a este curso.");

        aula.AssociarCurso(Id);
        _aulas.Add(aula);
    }

    private bool AulaExistente(Aula aula)
    {
        return _aulas.Any(a => a.Id == aula.Id);
    }

    public CursoDto ToDto()
    {
        return new CursoDto
        {
            Id = Id,
            Nome = Nome
        };
    }
}