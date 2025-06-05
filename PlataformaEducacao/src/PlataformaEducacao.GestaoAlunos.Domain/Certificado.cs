using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoAlunos.Domain;

public class Certificado : Entity
{
    public string NomeAluno { get; private set; }
    public string NomeCurso { get; private set; }
    public DateTime DataEmissao { get; private set; }
    public DateTime? DataConclusao { get; private set; }
    public string Descricao { get; private set; }
    public byte[] Arquivo { get; private set; }
    public Guid AlunoId { get; private set; }
    public Guid MatriculaId { get; private set; }

    // EF Relations
    public Aluno Aluno { get;  set; }
    public Matricula Matricula { get; set; }

    // Ef Constructor
    protected Certificado() { }
    public Certificado(string nomeAluno, string nomeCurso, Guid matriculaId, Guid alunoId, DateTime? dataConclusao)
    {
        NomeAluno = nomeAluno;
        NomeCurso = nomeCurso;
        MatriculaId = matriculaId;
        AlunoId = alunoId;
        DataEmissao = DateTime.Now;
        DataConclusao = dataConclusao;
        GerarDescricao();
        Validar();
    }

    private void GerarDescricao()
    {
        Descricao = $"Certificamos que o(a) aluno(a) {NomeAluno} concluiu o curso {NomeCurso} com sucesso no dia {DataConclusao:dd/MM/yyyy}.";
    }

    public void AdicionarArquivo(byte[] arquivoPdf)
    {
        if (arquivoPdf == null || arquivoPdf.Length == 0)
            throw new DomainException("Arquivo do certificado inválido.");

        Arquivo = arquivoPdf;
    }

    public void Validar()
    {   
        if (AlunoId == Guid.Empty)
            throw new DomainException("O campo AlunoId é obrigatório.");
        if (MatriculaId == Guid.Empty)
            throw new DomainException("O campo MatriculaId é obrigatório.");
        if (string.IsNullOrWhiteSpace(Descricao))
            throw new DomainException("O campo Descrição é obrigatório.");
        if (string.IsNullOrWhiteSpace(NomeAluno))
            throw new DomainException("O campo Nome Aluno é obrigatório.");
        if (string.IsNullOrWhiteSpace(NomeCurso))
            throw new DomainException("O campo Nome Curso é obrigatório.");
        if (!DataConclusao.HasValue)
            throw new DomainException("O campo Data Conclusão é obrigatório.");
    }
}