using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.GestaoConteudos.Domain;

namespace PlataformaEducacao.GestaoConteudos.Data.Mappings;

public class CursoMapping : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> builder)
    {
        builder.ToTable("Cursos");
        builder.HasKey(c => c.Id);

        // 1 : N - Curso : Aulas
        builder.HasMany(c => c.Aulas)
            .WithOne(c => c.Curso);
    }
}
public class AulaMapping : IEntityTypeConfiguration<Aula>
{
    public void Configure(EntityTypeBuilder<Aula> builder)
    {
        builder.ToTable("Aulas");
        builder.HasKey(a => a.Id);

        // 1 : N - Aula : Materiais
        builder.HasMany(a => a.Materiais)
            .WithOne(a => a.Aula);

        // 1 : N - Aula : ProgressoAula
        builder.HasMany(a => a.ProgressoAulas)
            .WithOne()
            .HasForeignKey(p => p.AulaId);
    }
}
public class MaterialMapping : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("Materiais");

        builder.HasKey(m => m.Id);
    }
}
public class ProgressoAulaMapping : IEntityTypeConfiguration<ProgressoAula>
{
    public void Configure(EntityTypeBuilder<ProgressoAula> builder)
    {
        builder.ToTable("ProgressoAulas");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.AlunoId)
            .IsRequired();

        builder.Property(p => p.AulaId)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion<short>();
    }
}