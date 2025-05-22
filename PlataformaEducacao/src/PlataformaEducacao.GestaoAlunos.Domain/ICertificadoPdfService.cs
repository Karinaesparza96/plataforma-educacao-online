namespace PlataformaEducacao.GestaoAlunos.Domain;

public interface ICertificadoPdfService
{
    byte[] GerarPdf(Certificado certificado);
}