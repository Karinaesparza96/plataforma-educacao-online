using PlataformaEducacao.Core.DomainObjects;

namespace PlataformaEducacao.GestaoAlunos.Domain;

public class Aluno(string nome) : Usuario
{
    public string Nome { get; private set; } = nome;

    private readonly List<Matricula> _matriculas = [];

    private readonly List<Certificado> _certificados = [];
    public IReadOnlyCollection<Matricula> Matriculas => _matriculas;

    public IReadOnlyCollection<Certificado> Certificados => _certificados;

    public void AdicionarMatricula(Matricula matricula)
    {
        if (MatriculaExistente(matricula))
            throw new DomainException("Matrícula já existente.");

        matricula.AguardarPagamento();

        _matriculas.Add(matricula);
    }

    public void AdicionarCertificado(Certificado certificado)
    {
        if (CertificadoExistente(certificado))
            throw new DomainException("Certificado já existente.");

        _certificados.Add(certificado);
    }

    private bool CertificadoExistente(Certificado certificado)
    {
        return _certificados.Any(c => c.Id == certificado.Id);
    }

    private bool MatriculaExistente(Matricula matricula)
    {
        return _matriculas.Any(m => m.Id == matricula.Id);
    }
}