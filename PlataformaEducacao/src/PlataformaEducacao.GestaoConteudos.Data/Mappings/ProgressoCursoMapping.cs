using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Data.Mappings;

public class ProgressoCursoMapping : IEntityTypeConfiguration<ProgressoCurso>
{
    public void Configure(EntityTypeBuilder<ProgressoCurso> builder)
    {
        builder.ToTable("ProgressoCursos");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.AlunoId)
            .IsRequired();
        builder.Property(p => p.CursoId)
            .IsRequired();

        builder.Property(p => p.PercentualConcluido)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Ignore(p => p.CursoConcluido);

        builder.HasIndex(p => new { p.CursoId, p.AlunoId })
            .IsUnique();
    }
}