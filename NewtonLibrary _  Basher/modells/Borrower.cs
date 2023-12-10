using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NewtonLibrary____Basher.modells
{
    internal class Borrower
    {
        public int Id { get; set; }
        [MaxLength(50)]

        public string? Firstname { get; set; }
        [MaxLength(50)]
        public string? Lastname { get; set; }
        public ICollection<BookLoan> BookLoans { get; set; }
    }
}
