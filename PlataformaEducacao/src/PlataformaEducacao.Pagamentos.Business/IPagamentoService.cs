using PlataformaEducacao.Core.DomainObjects.DTOs;

namespace PlataformaEducacao.Pagamentos.Business;

public interface IPagamentoService
{
    Task<bool> RealizarPagamentoCurso(PagamentoCurso pagamentoCurso);
}