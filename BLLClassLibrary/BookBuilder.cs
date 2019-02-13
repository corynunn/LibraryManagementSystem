using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLClassLibrary
{
    public class BookBuilder: IAuthorBuilderable
    {
        BookBLLCollection books;
        PersonBLLCollection people;
        //Used for author generation
        string authorFirstName;
        string authorLastName;
        public AuthorBuilder AuthorBuilder { get; set; }

        //Used to populate the book fields
        string iSBN;
        //string title;
        //string description;
        //string publisher;
        //int? numPages;
        //string yearPublished;
        //string language;
        //int numberCopies;
        int authorID;
        //string subject;

        public BookBLL Book { get; set; }
        
        public BookBuilder(BookBLLCollection Books, PersonBLLCollection People, string ISBN)
        {
            books = Books;
            people = People;
            iSBN = ISBN;

            Book = new BookBLL()
            {
                ISBN = iSBN
            };
        }

        /// <summary>
        /// Checks the given names, if the author exists the ID will be retrived now, or a new author will be created.
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        public void CheckNames(string FirstName, string LastName)
        {
            authorFirstName = FirstName;
            authorLastName = LastName;
            List<PersonBLL> personBLLs = (from p in people
                                          where p.FirstName == authorFirstName && p.LastName == authorLastName
                                          select p).ToList();
            if (personBLLs.Count == 0)
            {
                //There is no one in the collection with this name, this is an entirely new person
                AuthorBuilder = new AuthorBuilder(people, authorFirstName, authorLastName);
                AuthorBuilder.IsNewPerson = true;
            }
            else
            {
                bool authorFound = false;
                //There is someone with this name combination, check if they are already an author
                foreach (PersonBLL p in personBLLs)
                {
                    
                    if (p is AuthorBLL a)
                    {
                        //An author with this name was found.
                        authorFound = true;
                        authorID = p.ID;
                        Book.AuthorID = p.ID;
                        
                    }
                }
                if (!authorFound)
                {
                    AuthorBuilder = new AuthorBuilder(people, authorFirstName, authorLastName);
                    AuthorBuilder.IsNewAuthor = true;
                    authorID = personBLLs[0].ID;
                    Book.AuthorID = authorID;
                    AuthorBuilder.SetID(authorID);
                }

            }
        }

        /// <summary>
        /// Sets the book title, this is a required field.
        /// </summary>
        /// <param name="Title"></param>
        public void SetTitle(string Title)
        {
            Book.Title = Title;
        }

        /// <summary>
        /// Sets the NumberOfCopies for the book, this is a required field.
        /// </summary>
        /// <param name="Copies"></param>
        public void SetCopies(int Copies)
        {
            Book.NumberOfCopies = Copies;
        }

        /// <summary>
        /// Sets the number of pages, optional field.
        /// </summary>
        /// <param name="Pages"></param>
        public void SetPages(int Pages)
        {
            Book.NumPages = Pages;
        }

        /// <summary>
        /// Sets the subject, optional field.
        /// </summary>
        /// <param name="Subject"></param>
        public void SetSubject(string Subject)
        {
            Book.Subject = Subject;
        }

        /// <summary>
        /// Sets the description, optional field.
        /// </summary>
        /// <param name="Description"></param>
        public void SetDescription(string Description)
        {
            Book.Description = Description;
        }

        /// <summary>
        /// Sets the Publisher, optional field.
        /// </summary>
        /// <param name="Publisher"></param>
        public void SetPublisher(string Publisher)
        {
            Book.Publisher = Publisher;
        }

        /// <summary>
        /// Sets yearPublished, optional field.
        /// </summary>
        /// <param name="Year"></param>
        public void SetYear(string Year)
        {
            Book.YearPublished = Year;
        }

        /// <summary>
        /// Sets language, optional field.
        /// </summary>
        /// <param name="Language"></param>
        public void SetLanguage(string Language)
        {
            Book.Language = Language;
        }

        public void SetBio(string bio)
        {
            AuthorBuilder.SetBio(bio);
        }

        ///// <summary>
        ///// Finishes the book create process.
        ///// </summary>
        //public void CreateBook()
        //{
        //    //Check to see if an author needs to be created
        //    if (AuthorBuilder != null)
        //    {
        //        AuthorBuilder.CreateAuthor();
        //    }
        //    Book = new BookBLL()
        //    {
        //        ISBN = iSBN,
        //        Title = title,
        //        AuthorID = authorID,
        //        BookID = bookID,
        //        NumberOfCopies = numberCopies,
        //        NumPages = numPages,
        //        Description = description,
        //        Language = language,
        //        Publisher = publisher,
        //        Subject = subject,
        //        YearPublished = yearPublished
        //    };
        //}
    }
}
