using Microsoft.EntityFrameworkCore;
using NewtonLibrary____Basher.modells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewtonLibrary____Basher.Data
{
        internal class Context : DbContext
        {
            public DbSet<Author> Authors { get; set; }
            public DbSet<BookLoan> BookLoans { get; set; }

            public DbSet<Book> Books { get; set; }

            public DbSet<Borrower> Borrowers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(connectionString: "Server=localhost; Database=NewtonLibrary____Basher; Trusted_Connection=True; Trust Server Certificate =Yes; User Id=NewtonLibrary____Basher; password=Husby.001");
            }
        }
    }
