using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoConteudos.Domain;
public class Curso : Entity, IAggregateRoot
{
    public string Nome { get; private set; }
    public string ConteudoProgramatico { get; private set; }
    public Guid UsuarioCriacaoId { get; private set; }
    public decimal Preco { get; private set; }

    private readonly List<Aula> _aulas;
    public IReadOnlyCollection<Aula> Aulas => _aulas;

    public Curso(string nome, string conteudoProgramatico, Guid usuarioCriacaoId, decimal preco)
    {
        Nome = nome;
        ConteudoProgramatico = conteudoProgramatico;
        UsuarioCriacaoId = usuarioCriacaoId;
        Preco = preco;
        _aulas = [];

        Validar();
    }
    
    public void AdicionarAula(Aula aula)
    {
        if (AulaExistente(aula))
            throw new DomainException("Aula já associada a este curso.");

        aula.AssociarCurso(Id);
        _aulas.Add(aula);
    }

    public void AtualizarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome do curso é obrigatório.");
        Nome = nome;
    }
    public void AtualizarConteudo(string conteudoProgramatico)
    {
        if (string.IsNullOrWhiteSpace(conteudoProgramatico))
            throw new DomainException("O conteúdo programático é obrigatório.");
        ConteudoProgramatico = conteudoProgramatico;
    }
    public void AtualizarPreco(decimal preco)
    {
        if (preco <= 0)
            throw new DomainException("O preço do curso deve ser maior que zero.");
        Preco = preco;
    }

    private void Validar()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new DomainException("O nome do curso é obrigatório.");
        if (string.IsNullOrWhiteSpace(ConteudoProgramatico))
            throw new DomainException("O conteúdo programático é obrigatório.");
        if (UsuarioCriacaoId == Guid.Empty)
            throw new DomainException("O ID do usuário criador é obrigatório.");
        if (Preco <= 0)
            throw new DomainException("O preço do curso deve ser maior que zero.");
    }

    private bool AulaExistente(Aula aula)
    {
        return _aulas.Any(a => a.Id == aula.Id);
    }
}