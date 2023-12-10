using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewtonLibrary____Basher.modells
{
    internal class BookLoan
    {
        public int Id { get; set; }
        public int pin { get; set; } = new Random().Next(1000, 9999);
        public int BorrowerId { get; set; }  // Foreign key
        public ICollection<Book>? Books { get; set; } = new List<Book>();
        public Borrower? Borrower { get; set; }
    }
}
