using PlataformaEducacao.Core.DomainObjects.Enums;

namespace PlataformaEducacao.Core.DomainObjects.DTOs;

public class AulaDto
{
    public Guid Id { get; set; }
    public Guid CursoId { get; set; }
    public string Nome { get; set; }
    public string Conteudo { get; set; }
    public EProgressoAulaStatus Status { get; set; }
}