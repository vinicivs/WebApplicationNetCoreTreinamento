using WebApplicationMinimalAPINetCoreTreinamento.Domain.Entities;
using WebApplicationMinimalAPINetCoreTreinamento.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace WebApplicationMinimalAPINetCoreTreinamento.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Cep> Cep
        {
            get; set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cep>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsFixedLength();

                entity.Property(e => e.Logradouro)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Numero)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Complemento)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Bairro)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Cidade)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Uf)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsFixedLength();

                //entity.Property(e => e.Pais)
                //    .IsRequired()
                //    .HasMaxLength(50);

                //entity.Property(e => e.Complemento)
                //    .HasMaxLength(200);

                //entity.Property(e => e.UnidadeFederativa)
                //    .HasMaxLength(100);

                //entity.Property(e => e.Ibge)
                //    .HasMaxLength(7);

                //entity.Property(e => e.Gia)
                //    .HasMaxLength(20);

                //entity.Property(e => e.Ddd)
                //    .HasMaxLength(3);

                //entity.Property(e => e.Siafi)
                //    .HasMaxLength(6);

                //entity.Property(e => e.UltimaAtualizacao)
                //    .IsRequired();

                //entity.Property(e => e.Versao)
                //    .IsRequired()
                //    .HasDefaultValue(1);

                //entity.Property(e => e.CreatedBy)
                //    .HasMaxLength(100);

                //entity.Property(e => e.UpdatedBy)
                //    .HasMaxLength(100);

                //entity.HasIndex(e => e.Codigo)
                //    .IsUnique()
                //    .HasDatabaseName("IX_Ceps_Codigo");

                //entity.HasIndex(e => new { e.Cidade, e.Estado })
                //    .HasDatabaseName("IX_Ceps_Cidade_Estado");

                //entity.HasIndex(e => e.Bairro)
                //    .HasDatabaseName("IX_Ceps_Bairro");

                //entity.HasIndex(e => e.Ativo)
                //    .HasDatabaseName("IX_Ceps_Ativo");

                //entity.HasQueryFilter(e => e.Ativo);
            });

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.SetCreatedBy("system");
                        break;
                    case EntityState.Modified:
                        entry.Entity.SetUpdatedBy("system");
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
