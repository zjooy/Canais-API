using Canais.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Canais.Infrastructure;

public class CanaisDbContext : DbContext
{
    public CanaisDbContext(DbContextOptions<CanaisDbContext> options) : base(options) { }

    public DbSet<ReclamacoesEntity> Reclamacoes { get; set; }
    public DbSet<CategoriasEntity> Categorias { get; set; }
    public DbSet<ReclamacaoCategoriasEntity> ReclamacaoCategorias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReclamacoesEntity>(entity =>
        {
            entity.ToTable("tb_reclamacoes");
            entity.HasKey(e => e.IdReclamacao);

            entity.HasMany(r => r.ReclamacaoCategorias)
              .WithOne(rc => rc.Reclamacao)
              .HasForeignKey(rc => rc.IdReclamacao)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReclamacoesEntity>()
              .Property(r => r.Anexos)
              .HasColumnType("jsonb");
        });

        modelBuilder.Entity<CategoriasEntity>(entity =>
        {
            entity.ToTable("tb_categorias");
            entity.HasKey(e => e.Id);

            entity.HasMany(c => c.ReclamacaoCategorias)
              .WithOne(rc => rc.Categorias)
              .HasForeignKey(rc => rc.CategoriaId)
              .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ReclamacaoCategoriasEntity>(entity =>
        {
            entity.ToTable("tb_reclamacaocategoria");
            entity.HasKey(e => e.IdReclamacao);
        });

        base.OnModelCreating(modelBuilder);
    }
}
