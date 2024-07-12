using KarapinhaXpto.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.DAL
{
    public class KarapinhaXptoDbContext : DbContext
    {
        public KarapinhaXptoDbContext(DbContextOptions<KarapinhaXptoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         
            modelBuilder.Entity<ProfissionalCategoria>()
                .HasKey(ps => new { ps.ProfissionalId, ps.CategoriaID });
        }

        public DbSet<Perfil> Perfils { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<ProfissionalCategoria> ProfissionalCategorias { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Marcacao> Marcacaos { get; set; }
        public DbSet<Profissional> Profissionals { get; set; }
        public DbSet<ProfissionalHorario> ProfissionalHorarios { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<ServicoMarcacao> ServicoMarcacaos { get; set; }
        public DbSet<Utilizador> Utilizadors { get; set; }
    }
}
