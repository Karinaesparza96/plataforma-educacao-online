using iTextSharp.text;
using iTextSharp.text.pdf;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Aplication.Services;

public class CertificadoService : ICertificadoPdfService
{
    public byte[] GerarPdf(Certificado certificado)
    {
        using var memoryStream = new MemoryStream();

        var document = new Document(PageSize.A4.Rotate(), 50, 50, 50, 50);
        var writer = PdfWriter.GetInstance(document, memoryStream);

        document.Open();

        var tituloFonte = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 22);
        var titulo = new Paragraph("Certificado de Conclusão", tituloFonte)
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingAfter = 40f
        };
        document.Add(titulo);

        var corpoFonte = FontFactory.GetFont(FontFactory.HELVETICA, 14);
        var corpo = new Paragraph(certificado.Descricao, corpoFonte)
        {
            Alignment = Element.ALIGN_CENTER
        };
        document.Add(corpo);

        var dataFonte = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 12);
        var dataTexto = new Paragraph($"Emitido em: {DateTime.Now:dd 'de' MMMM 'de' yyyy}", dataFonte)
        {
            Alignment = Element.ALIGN_RIGHT,
            SpacingBefore = 50
        };
        document.Add(dataTexto);

        document.Close();
        return memoryStream.ToArray();
    }
}
