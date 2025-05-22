using PlataformaEducacao.Core.DomainObjects.Enums;

namespace PlataformaEducacao.Core.DomainObjects.DTOs;

public class MatriculaDto
{
    public Guid Id { get; set; }
    public EStatusMatricula Status { get; set; }
}