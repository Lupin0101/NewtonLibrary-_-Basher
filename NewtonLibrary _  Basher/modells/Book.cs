using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NewtonLibrary____Basher.modells
{
    internal class Book
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string? Titel { get; set; }
        public int? Year { get; set; }



        public bool Loaned { get; set; } = false;
       
        public DateTime? LoanDate { get; set; }

      

        public DateTime? ReturnDate { get;  set; } 
        public Guid Isbn { get; set; } = Guid.NewGuid();

        public int Grade { get; set; } = new Random().Next(1, 5);


        public int? LoanCardId { get ; set; }      
       
        public BookLoan? BookLoan { get; set; }

        public ICollection<Author>? Authors { get; set; }
    }
}

