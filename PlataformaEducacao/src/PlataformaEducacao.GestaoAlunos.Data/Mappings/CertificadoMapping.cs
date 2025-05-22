using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Data.Mappings;

public class CertificadoMapping : IEntityTypeConfiguration<Certificado>
{
    public void Configure(EntityTypeBuilder<Certificado> builder)
    {
        builder.ToTable("Certificados");
        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.Aluno);
        builder.HasOne(c => c.Matricula);
    }
}