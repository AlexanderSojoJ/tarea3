using System.Data.Entity;

namespace tarea4
{
    public class TP3Context : DbContext
    {
        public TP3Context() : base("name=TP3")
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Numero> Numeros { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configurar la relación uno a muchos
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.Numeros)
                .WithRequired(n => n.Producto)
                .HasForeignKey(n => n.ProductoId);

            base.OnModelCreating(modelBuilder);
        }
    }
}