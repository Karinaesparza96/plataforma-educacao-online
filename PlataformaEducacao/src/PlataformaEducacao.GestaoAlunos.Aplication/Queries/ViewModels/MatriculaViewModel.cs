namespace PlataformaEducacao.GestaoAlunos.Aplication.Queries.ViewModels;

public class MatriculaViewModel
{
    public Guid Id { get; set; } 
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }
    public int Status { get; set; }
    public string StatusDescricao { get; set; }
    public DateTime? DataMatricula { get; set; }
}