using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLClassLibrary
{
    public class BookUpdater: IAuthorBuilderable
    {
        BookBLLCollection books;
        PersonBLLCollection people;
        CheckOutLogBLLCollection logs;

        //Original author names, used by the ui and CheckNames method
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        //If a new Author needs to be created
        public AuthorBuilder AuthorBuilder { get; set; }

        //Properties used to change the book record
        public string ISBN { get; private set; }
        public string Title { get; private set; }
        public int AuthorID { get; set; }
        public int? NumPages { get; private set; }
        public int NumberOfCopies { get; private set; }
        public string Description { get; private set; }
        public string Publisher { get; private set; }
        public string YearPublished { get; private set; }
        public string Subject { get; private set; }
        public string Language { get; private set; }

        //Reference to the BookBLL to be changed
        public BookBLL Book { get; set; }

        //Used to determine which fields are to be updated
        public bool ChangeISBN { get; private set; }
        public bool ChangeTitle { get; private set; }
        public bool ChangeAuthorID { get; private set; }
        public bool ChangeNumPages { get; private set; }
        public bool ChangeNumberOfCopies { get; private set; }
        public bool ChangeDescription { get; private set; }
        public bool ChangePublisher { get; private set; }
        public bool ChangeYearPublished { get; private set; }
        public bool ChangeSubject { get; private set; }
        public bool ChangeLanguage { get; private set; }


        public bool RemoveCopies { get; private set; }

        public BookUpdater(BookBLLCollection Books, PersonBLLCollection People, CheckOutLogBLLCollection Logs, BookBLL targetBook)
        {
            books = Books;
            people = People;
            logs = Logs;
            Book = targetBook;
            GetNames();
        }

        public BookUpdater(BookBLLCollection Books, PersonBLLCollection People, CheckOutLogBLLCollection Logs, BookBLL targetBook, bool remove) : this(Books, People, Logs, targetBook)
        {
            RemoveCopies = remove;
        }

        void GetNames()
        {
            PersonBLL author = people[Book.AuthorID];
            FirstName = author.FirstName;
            LastName = author.LastName;
        }

        /// <summary>
        /// Will update the BookBLL that is in the custom collection.
        /// </summary>
        public void FinalizeChanges()
        {
            if (ChangeISBN)
            {
                Book.ISBN = ISBN;
            }
            if (ChangeTitle)
            {
                Book.Title = Title;
            }
            if (ChangeAuthorID)
            {
                Book.AuthorID = AuthorID;
            }
            if (ChangeNumPages)
            {
                Book.NumPages = NumPages;
            }
            if (ChangeNumberOfCopies)
            {
                Book.NumberOfCopies = NumberOfCopies;
            }
            if (ChangeDescription)
            {
                Book.Description = Description;
            }
            if (ChangePublisher)
            {
                Book.Publisher = Publisher;
            }
            if (ChangeYearPublished)
            {
                Book.YearPublished = YearPublished;
            }
            if (ChangeSubject)
            {
                Book.Subject = Subject;
            }
            if (ChangeLanguage)
            {
                Book.Language = Language;
            }
        }

        public void UpdateAuthorID(string firstName, string lastName)
        {
            //First, retirve the names of the author already associated with the book
            
            if (firstName != FirstName || lastName != LastName)
            {
                //The author entered is different from the current author
                //Find the ID of the new author
                AuthorSearch search = new AuthorSearch(people, firstName, lastName);
                if (search.Author == null)
                {
                    AuthorBuilder = new AuthorBuilder(people, firstName, lastName);
                    if (search.FoundPersons.Count == 0)
                    {
                        //This is an entirely new person
                        AuthorBuilder.IsNewPerson = true;
                    }
                    else
                    {
                        //This person is already in the database, but not as an author yet.
                        AuthorBuilder.IsNewAuthor = true;
                        AuthorID = search.FoundPersons[0].ID;
                        AuthorBuilder.SetID(AuthorID);
                    }
                }
                else
                {
                    AuthorID = search.Author.ID;
                }
                ChangeAuthorID = true;
            }
        }

        /// <summary>
        /// Checks to make sure the ISBN is unique then sets the ISBN for changing.
        /// </summary>
        /// <param name="x"></param>
        public void UpdateISBN(string x)
        {
            if (x != Book.ISBN)
            {
                BookSearch search = new BookSearch(books, people, x);
                search.ISBNSearch();
                if (search.FoundBooks.Count !=0)
                {
                    throw new Exception($"ISBN must be unique, desired value is used by: {search.FoundBooks[0].Title}");
                }
                ISBN = x;
                ChangeISBN = true;
            }
        }

        /// <summary>
        /// Sets the title field for changing.
        /// </summary>
        /// <param name="x"></param>
        public void UpdateTitle(string x)
        {
            if (x != Book.Title)
            {
                Title = x;
                ChangeTitle = true;
            }
        }

        /// <summary>
        /// Sets the numPages field for changing.
        /// </summary>
        /// <param name="x"></param>
        public void UpdateNumPages(int? x)
        {
            if (x != Book.NumPages)
            {
                NumPages = x;
                ChangeNumPages = true;
            }
        }

        /// <summary>
        /// Sets the description field for changing.
        /// </summary>
        /// <param name="x"></param>
        public void UpdateDescription(string x)
        {
            if (x != Book.Description)
            {
                if (x == "")
                {
                    Description = null;
                }
                else
                {
                    Description = x;
                }
                ChangeDescription = true;
            }
        }

        /// <summary>
        /// Sets the pubisher fields for changing.
        /// </summary>
        /// <param name="x"></param>
        public void UpdatePublisher(string x)
        {
            if (x != Book.Publisher)
            {
                if (x == "")
                {
                    Publisher = null;
                }
                else
                {
                    Publisher = x;
                }
                ChangePublisher = true;
            }
        }

        /// <summary>
        /// Sets the YearPublished field for changing.
        /// </summary>
        /// <param name="x"></param>
        public void UpdateYearPub(string x)
        {
            if (x != Book.YearPublished)
            {
                if (x == "")
                {
                    YearPublished = null;
                }
                else
                {
                    YearPublished = x;
                }
                ChangeYearPublished = true;
            }
        }

        /// <summary>
        /// Sets the subject field for changing.
        /// </summary>
        /// <param name="x"></param>
        public void UpdateSubject(string x)
        {
            if (x != Book.Subject)
            {
                if (x == "")
                {
                    Subject = null;
                }
                else
                {
                    Subject = x;
                }
                ChangeSubject = true;
            }
        }

        /// <summary>
        /// Sets the language field for updating.
        /// </summary>
        /// <param name="x"></param>
        public void UpdateLanguage(string x)
        {
            if (x != Book.Language)
            {
                if (x == "")
                {
                    Language = null;
                }
                else
                {
                    Language = x;
                }
                ChangeLanguage = true;
            }
        }

        /// <summary>
        /// Checks logic of number of copies and attempts to set the value, will set flag used to update the database.
        /// </summary>
        /// <param name="x"></param>
        public void UpdateNumberOfCopies(int x)
        {
            //The number of copies cannot be reduced below the amount of checked out copies
            int minimum = Book.NumberOfCopies - logs.AvailableCopies(Book);
            if (x < minimum)
            {
                throw new Exception("You cannot reduce the number of copies by that much.");
            }
            NumberOfCopies = x;
            ChangeNumberOfCopies = true;
        }

        /// <summary>
        /// Handles logic for changing the number of copies by addition and subtraction.
        /// </summary>
        /// <param name="logs"></param>
        /// <param name="x"></param>
        public void AddOrSubtractCopies(int x)
        {
            if (RemoveCopies == true)
            {
                //To remove copies we must check the availablity logic
                if (x > logs.AvailableCopies(Book))
                {
                    throw new Exception("You cannot reduce the number of copies by that much.");
                }
                x *= -1;
            }
            NumberOfCopies = Book.NumberOfCopies;
            NumberOfCopies += x;
            ChangeNumberOfCopies = true;
            books.UpdateBookFields(this);
        }

        /// <summary>
        /// Used for updating books if the user inputs invalid values.
        /// </summary>
        public void Reset()
        {
            ChangeAuthorID = false;
            ChangeDescription = false;
            ChangeISBN = false;
            ChangeLanguage = false;
            ChangeNumberOfCopies = false;
            ChangeNumPages = false;
            ChangePublisher = false;
            ChangeSubject = false;
            ChangeTitle = false;
            ChangeYearPublished = false;
        }

        /// <summary>
        /// Sets the bio field of the Author being built.
        /// </summary>
        /// <param name="bio"></param>
        public void SetBio(string bio)
        {
            AuthorBuilder.SetBio(bio);
        }
    }
}
