using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaEducacao.GestaoAlunos.Domain;

namespace PlataformaEducacao.GestaoAlunos.Data.Mappings;

public class MatriculaMapping : IEntityTypeConfiguration<Matricula>
{
    public void Configure(EntityTypeBuilder<Matricula> builder)
    {
        builder.ToTable("Matriculas");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.CursoId)
            .IsRequired();
        builder.Property(m => m.Status)
            .IsRequired();
    }
}