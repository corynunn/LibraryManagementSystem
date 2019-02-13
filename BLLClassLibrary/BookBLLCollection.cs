using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseFirstLibraryApp;

namespace BLLClassLibrary
{
    public class BookBLLCollection : IEnumerable<BookBLL>
    {
        List<BookBLL> books = new List<BookBLL>();

        public BookBLL this[int index]
        {
            get
            {
                return GetBook(index);
            }
            set
            {
                books.Add(value);
            }
        }

        /// <summary>
        /// Adds the given book to the collection.
        /// </summary>
        /// <param name="book"></param>
        public void Add(BookBLL book)
        {
            books.Add(book);
            books.Sort();
        }

        /// <summary>
        /// Finds the book in the database with the same BookID and matches the fields to the BookBLL passed in.
        /// </summary>
        /// <param name="updater"></param>
        public void UpdateBookFields(BookUpdater updater)
        {
            //First check if a new author needs to be created
            if (updater.AuthorBuilder != null)
            {
                AddAuthorToDatabase(updater);
            }
            using (LibraryDBEntities context = new LibraryDBEntities())
            {
                Book book = (from b in context.Books
                             where b.BookID == updater.Book.BookID
                             select b).FirstOrDefault();
                //Update the fields of the book record to match any new data
                if (updater.ChangeAuthorID)
                {
                    if(updater.AuthorBuilder != null && updater.AuthorBuilder.IsNewPerson)
                    {
                        updater.AuthorID = updater.Book.AuthorID;
                    }
                    book.AuthorID = updater.AuthorID;
                }

                if (updater.ChangeTitle)
                    book.Title = updater.Title;
                if (updater.ChangeISBN)
                    book.ISBN = updater.ISBN;
                if (updater.ChangeLanguage)
                    book.Language = updater.Language;
                if (updater.ChangeNumberOfCopies)
                    book.NumberOfCopies = updater.NumberOfCopies;
                if (updater.ChangeNumPages)
                    book.NumPages = updater.NumPages;
                if (updater.ChangePublisher)
                    book.Publisher = updater.Publisher;
                if (updater.ChangeDescription)
                    book.Description = updater.Description;
                if (updater.ChangeSubject)
                    book.Subject = updater.Subject;
                if (updater.ChangeYearPublished)
                    book.YearPublished = updater.YearPublished;

                context.SaveChanges();
            }
            updater.FinalizeChanges();
        }


        void AddAuthorToDatabase(IAuthorBuilderable builder)
        {
            if (builder.AuthorBuilder.IsNewPerson)//A new person needs to be added, too
            {
                using (LibraryDBEntities context = new LibraryDBEntities())
                {
                    Person newPerson = new Person()
                    {
                        FirstName = builder.AuthorBuilder.Author.FirstName,
                        LastName = builder.AuthorBuilder.Author.LastName
                    };
                    context.People.Add(newPerson);
                    context.SaveChanges();
                }
                //Get the id of the new person
                using (LibraryDBEntities context = new LibraryDBEntities())
                {
                    Person person = (from p in context.People
                                     where p.FirstName == builder.AuthorBuilder.Author.FirstName && p.LastName == builder.AuthorBuilder.Author.LastName
                                     select p).FirstOrDefault();
                    builder.AuthorBuilder.SetID(person.PersonID);
                    builder.Book.AuthorID = person.PersonID;
                }
            }
            //Now a new author can be added
            using (LibraryDBEntities context = new LibraryDBEntities())
            {
                builder.AuthorBuilder.CreateAuthor();
                Author newAuthor = new Author()
                {
                    ID = builder.AuthorBuilder.Author.ID,
                    Bio = builder.AuthorBuilder.Author.Bio
                };
                context.Authors.Add(newAuthor);
                context.SaveChanges();
            }
            //Now the authorBLL is ready to be added to the collection
            builder.AuthorBuilder.AddAuthorToCollection();
        }

        /// <summary>
        /// Adds the book to the database, if the author is new they will be added first.
        /// </summary>
        /// <param name="builder"></param>
        public void AddBookToDatabase(BookBuilder builder)
        {
            if (builder.AuthorBuilder != null)//A new author needs to be added
            {
                
                AddAuthorToDatabase(builder);
            }
            //Adding the book to the database
            using (LibraryDBEntities context = new LibraryDBEntities())
            {
                //Create the book and add to database
                Book newBook = new Book()
                {
                    ISBN = builder.Book.ISBN,
                    Title = builder.Book.Title,
                    AuthorID = builder.Book.AuthorID,
                    Subject = builder.Book.Subject,
                    Description = builder.Book.Description,
                    NumberOfCopies = builder.Book.NumberOfCopies,
                    NumPages = builder.Book.NumPages,
                    YearPublished = builder.Book.YearPublished,
                    Language = builder.Book.Language,
                    Publisher = builder.Book.Publisher
                };
                context.Books.Add(newBook);

                context.SaveChanges();
                //Get the book id
                builder.Book.BookID = newBook.BookID;
                //Add the BookBLL to the customcollection
                books.Add(builder.Book);
            }
            Sort();
        }
        /// <summary>
        /// Connects to the database and retrives a new list of books, saved as BookBLL.
        /// </summary>
        public void PopulateBooks()
        {
            using (LibraryDBEntities context = new LibraryDBEntities())
            {
                List<BookBLL> newBooks = new List<BookBLL>();
                BookBLL newBook = null;
                foreach (Book b in context.Books)
                {
                    newBook = new BookBLL()
                    {
                        BookID = b.BookID,
                        Title = b.Title,
                        ISBN = b.ISBN,
                        AuthorID = b.AuthorID,
                        NumPages = b.NumPages,
                        Subject = b.Subject,
                        Description = b.Description,
                        Publisher = b.Publisher,
                        YearPublished = b.YearPublished,
                        Language = b.Language,
                        NumberOfCopies = b.NumberOfCopies
                    };
                    newBooks.Add(newBook);
                }
                books = newBooks;
            }
            Sort();
        }

        /// <summary>
        /// Returns a reference of the books list.
        /// </summary>
        /// <returns></returns>
        public List<BookBLL> GetBooks()
        {
            List<BookBLL> bookList = new List<BookBLL>();
            bookList.AddRange(books);
            return bookList;
        }

        /// <summary>
        /// Sorts the books by Title then ISBN
        /// </summary>
        public void Sort()
        {
            books.Sort();
        }

        /// <summary>
        /// Returns the request for book based on the BookID.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        BookBLL GetBook(int index)
        {
            for (int j = 0; j < books.Count; j++)
            {
                if (books[j].BookID == index)
                {
                    return books[j];
                }
            }
            throw new Exception("Tried to access book out of range.");
        }

        #region IEnumerable Support
        IEnumerator<BookBLL> IEnumerable<BookBLL>.GetEnumerator()
        {
            foreach (BookBLL b in books)
            {
                yield return b;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this;
        }
        #endregion
    }
}
