using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Queries.ViewModels;

public class MatriculaViewModel
{
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }
    public EStatusMatricula Status { get; set; }
    public DateTime DataMatricula { get; set; }
}