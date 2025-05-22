using PlataformaEducacao.Core.DomainObjects.DTOs;

namespace PlataformaEducacao.Core.DomainObjects;

public interface IPagamentoService
{
    Task<bool> RealizarPagamentoCurso(PagamentoCurso pagamentoCurso);
}