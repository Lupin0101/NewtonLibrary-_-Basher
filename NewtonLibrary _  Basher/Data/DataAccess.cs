using Microsoft.EntityFrameworkCore;
using NewtonLibrary____Basher.modells;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;
using static NewtonLibrary____Basher.Data.Context;

namespace NewtonLibrary____Basher.Data
{
   
    
        
    public enum BookTitle
        {
            [Description("Metro 2033")] Metro, [Description("Lord of the rings")] Lotr, [Description("Judge Dredd")] Dredd,
            [Description("Lupin")] GOT, [Description("Silent Hill")] SH, [Description("Batman Bin Suparman")] FMARVEL, Halo,
            [Description("The Picture of Dorian Gray")] DG, [Description("Never Let Me Go")] Never, [Description("The Road")] VÄG, [Description("March: Book One (Oversized Edition)")] Stor, [Description("The Hobbit")] Liten,
            [Description("I am Zlatan")] TroddeDettaVaEnFilm, [Description("A Tale of Two Cities")] SimCity, [Description("Crime and Punishment")] HörtDennaVaBra, [Description("We Should All Be Feminists")] IVissaFall, [Description("Persepolis")] NuräckerD,
        }

        public class DataAccess
        {
            Context context = new Context();

        public void ReturnBook(int bookId)
        {
            using (var context = new Context())
            {
                var book = context.Books.Include(b => b.BookLoan).FirstOrDefault(b => b.Id == bookId);

                if (book != null)
                {
                    // Check if the book is currently on loan
                    if (book.Loaned)
                    {
                        // Explicitly load the BookLoan navigation property
                        context.Entry(book).Reference(b => b.BookLoan).Load();

                        // Update loan-related information to reflect that the book has been returned
                        book.LoanCardId = null;
                        book.ReturnDate = DateTime.Now;  // Set the return date
                        book.Loaned = false;

                        // Save changes
                        context.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine($"Book #{bookId} is not currently on loan.");
                    }
                }
            }
        }
        public void AddBookIdToPersonLoanCard(int personId, int bookId)
        {
            using (var context = new Context())
            {
                var person = context.Borrowers.Include(p => p.BookLoans).SingleOrDefault(p => p.Id == personId);

                if (person == null)
                {
                    return;
                }

                if (person.BookLoans == null)
                {
                    Console.WriteLine($"Person #{personId} has no LoanCard");
                    return;
                }

                var book = context.Books.Find(bookId);

                if (book != null)
                {
                    // Assuming you want to work with the first book loan of the person
                    var bookLoan = person.BookLoans.FirstOrDefault();

                    if (bookLoan != null)
                    {
                        book.LoanCardId = bookLoan.Id;
                        book.Loaned = true;
                        book.LoanDate = DateTime.Now;
                        //book.ReturnDate = book.LoanDate?.AddDays(14);
                        context.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine($"Person #{personId} has no book loans");
                    }
                }
                else
                {
                    Console.WriteLine($"No book with id #{bookId}\nReturn nothing!");
                }
            }
        }

        public void seed()
            {

            Author author = new Author();
            author.FirstName = "Husby";
            author.LastName = "blabla";

            
            Book book = new Book();
            book.Titel = "Min bok";
            book.Year = 1;


            author.books.Add(book);

            Borrower borrower = new Borrower();
            borrower.Firstname = "Ali";
            borrower.Lastname = "wahlström";



            context.Books.Add(book);
            context.Authors.Add(author);
            context.Borrowers.Add(borrower);

            context.SaveChanges();
                

            

            }




            public csSeedGenerator rnd = new csSeedGenerator();

            public void CreateFiller()
            {
                using (var context = new Context())
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Borrower user = new Borrower();

                        user.Firstname = rnd.FirstName;
                        user.Lastname = rnd.LastName;

                        Book books = new Book();
                        books.Year = rnd.Next(1900, 2023);
                        books.Titel = GetEnumDescription(rnd.FromEnum<BookTitle>());
                        BookLoan bookloan = new BookLoan();

                        Author author = new Author();

                        author.FirstName = rnd.FirstName;
                        author.LastName = rnd.LastName;

                        context.Borrowers.Add(user);
                        context.Books.Add(books);
                        context.Authors.Add(author);
                    }

                    context.SaveChanges();
                }
            }

            public void MarkBookAsNotLoaned(int bookId)
            {
                using (var context = new Context())
                {
                    var book = context.Books.Include(b => b.BookLoan).FirstOrDefault(b => b.Id == bookId);

                    if (book != null)
                    {
                        book.LoanCardId = null;
                        if (book.BookLoan != null)
                        {
                            book.BookLoan.Books.Remove(book);
                        }
                        context.SaveChanges();
                    }
                }
            }

            public void AddPersonToDatabase(string firstName, string lastName)
            {
                using (var context = new Context())
                {
                    var user = new Borrower
                    {
                        Firstname = firstName,
                        Lastname = lastName
                    };
                    context.Borrowers.Add(user);
                    context.SaveChanges();
                }
            }
        public void AddAuthorToDatabase(string Name , string Name2)
        {
            using (var context = new Context())
            {
                var author = new Author { FirstName = Name , LastName = Name2 };
               
                context.Authors.Add(author);
                context.SaveChanges();
            }
        }

        public void AddBookToDatabase(string title, List<string> authorNames, params int[] authorId)
            {
                using (var context = new Context())
                {
                    var author = context.Authors.Where(a => authorId.Contains(a.Id)).ToList();

                    var book = new Book
                    {
                        Titel = title,
                        Authors = author,
                        Year = new Random().Next(1900, 2023)
                    };
                    context.Books.Add(book);
                    context.SaveChanges();
                }
            }

        public void AddLoanCardToPerson(int id)
        {
            using (var context = new Context())
            {
                var user = context.Borrowers.Include(b => b.BookLoans).FirstOrDefault(b => b.Id == id);

                if (user == null)
                {
                    return;
                }

                var userCard = new BookLoan();

                // Ensure the relationship between Borrower and BookLoan is established
                user.BookLoans.Add(userCard);
                userCard.Borrower = user;  // Set the navigation property on BookLoan

                context.SaveChanges();
            }
        }


        public void Clear()
            {
                using (var context = new Context())
                {
                    var allPersons = context.Borrowers.ToList();
                    context.Borrowers.RemoveRange(allPersons);
                    var allBooks = context.Books.ToList();
                    context.Books.RemoveRange(allBooks);
                    var allAuthors = context.Borrowers.ToList();
                    context.Borrowers.RemoveRange(allAuthors);
                    var allUserCards = context.BookLoans.ToList();
                    context.RemoveRange(allUserCards);
                    context.SaveChanges();
                }
            }

        public void RemovePerson(int personId)
        {
            Context context = new Context();
            var personToRemove = context.Borrowers.Find(personId);

            if (personToRemove != null)
            {
                context.Borrowers.Remove(personToRemove);
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Person is not present.");
            }

        }
        public void Removebook(int bookId)
        {
            Context context = new Context();
            var bookToRemove = context.Books.Find(bookId);

            if (bookToRemove != null)
            {
                context.Books.Remove(bookToRemove);
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("book is not present.");
            }

        }
        public void Removeauthor(int authorId)
        {
            Context context = new Context();
            var authorToRemove = context.Authors.Find(authorId);

            if (authorToRemove != null)
            {
                context.Authors.Remove(authorToRemove);
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("author is not present.");
            }

        }




        private string GetEnumDescription(Enum value)
            {
                var field = value.GetType().GetField(value.ToString());

                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    return attribute.Description;
                }
                return value.ToString();
            }

        }
    }

