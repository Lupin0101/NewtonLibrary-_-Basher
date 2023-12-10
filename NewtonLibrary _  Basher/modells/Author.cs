using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewtonLibrary____Basher.modells
{
    internal class Author
    {
        public int Id { get; set; }
        [MaxLength(50)]

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Book>? books { get; set; } = new List<Book>();
    }
}
