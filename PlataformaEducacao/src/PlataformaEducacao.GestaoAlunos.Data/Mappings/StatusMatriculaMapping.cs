using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Data.Mappings;

public class StatusMatriculaMapping : IEntityTypeConfiguration<StatusMatricula>
{
    public void Configure(EntityTypeBuilder<StatusMatricula> builder)
    {
        builder.HasKey(s => s.Id);
        builder.ToTable("StatusMatriculas");
    }
}