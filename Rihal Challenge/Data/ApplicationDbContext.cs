using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace Rihal_Challenge.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<StudentsClass> students { get; set; }
        public DbSet<ClassesClass> classes { get; set; }
        public DbSet<CountriesClass> countries { get; set; }
    }
}
   
