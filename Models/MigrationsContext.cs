using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace LB6.Models
{
    public partial class MigrationsContext : DbContext
    {
        public MigrationsContext()
            : base("name=MigrationsContext")
        {
        }

        public DbSet<Notebook> Notebook { get; set; }
        public DbSet<Brand> Brand{ get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
    public class Notebook
    {
        [Key]
        [Column("ID_notebook")]
        public System.Guid Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null;

        [ForeignKey("Brand")]
        public int BrandID { get; set; }

        public Brand Brand { get; set; }

        [StringLength(50)]
        public string Resolution { get; set; } = null;

        [StringLength(50)]
        public string Frequency { get; set; } = null;

        [StringLength(50)]
        public string Weight { get; set; } = null;
    }
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BrandID { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null;
    }
}
