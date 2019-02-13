using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLClassLibrary
{
    public class LibrarianMenuLists
    {
        PersonBLLCollection people;
        BookBLLCollection books;
        CheckOutLogBLLCollection logs;

        public LibrarianMenuLists(PersonBLLCollection People, BookBLLCollection Books, CheckOutLogBLLCollection Logs)
        {
            people = People;
            books = Books;
            logs = Logs;
        }

        /// <summary>
        /// Returns a list of librarianbll objects for display in the menu listbox.
        /// </summary>
        /// <returns></returns>
        public List<LibrarianBLL> GetLibrarians()
        {
            List<LibrarianBLL> librarians = new List<LibrarianBLL>();
            foreach (PersonBLL p in people)
            {
                if (p is LibrarianBLL l)
                {
                    librarians.Add(l);
                }
            }
            return librarians;
        }

        /// <summary>
        /// Returns a list of formatted string to display cardholder information and checked out books.
        /// </summary>
        /// <returns></returns>
        public List<string> GetCardholderInformation()
        {
            List<CardholderBLL> cardholders = new List<CardholderBLL>();
            List<string> stringArray = new List<string>();
            foreach (PersonBLL p in people)
            {
                if (p is CardholderBLL c)
                {
                    string output = c.ToString();
                    //get checkoutlogs that match the cardholder
                    List<CheckOutLogBLL> cardholdersLogs = logs.GetCheckOutLogBLLs(c.ID);
                    if (cardholdersLogs.Count > 0)
                    {
                        output += " Checked Out: ";
                        List<BookBLL> borrowedBooks = new List<BookBLL>();
                        foreach (CheckOutLogBLL l in cardholdersLogs)
                        {
                            BookBLL book = (from b in books
                                            where b.BookID == l.BookID
                                            select b).FirstOrDefault();
                            borrowedBooks.Add(book);
                            foreach (BookBLL b in borrowedBooks)
                            {
                                output += b.ToString() + $", Date Checked: {l.CheckOutDate}";
                                if (l.IsOverDue())
                                {
                                    output += " OVERDUE";
                                }
                            }
                        }
                    }
                    stringArray.Add(output);
                }
            }
            return stringArray;
        }

        /// <summary>
        /// Returns a list of formatted string displaying author and book information.
        /// </summary>
        /// <returns></returns>
        public List<string> GetAuthorInformation()
        {
            List<AuthorBLL> authors = new List<AuthorBLL>();
            List<string> stringList = new List<string>();
            foreach (PersonBLL p in people)
            {
                if (p is AuthorBLL a)
                {
                    string output = a.ToString();
                    //Find their books
                    List<BookBLL> authorsBooks = new List<BookBLL>();
                    authorsBooks = (from b in books
                                    where b.AuthorID == a.ID
                                    select b).ToList();
                    if (authorsBooks.Count > 0)
                    {
                        output += " Books: ";
                        authorsBooks.Sort();
                        foreach (BookBLL b in authorsBooks)
                        {
                            output += b.ToString();
                        }
                    }
                    stringList.Add(output);
                }
            }
            return stringList;
        }

        /// <summary>
        /// Returns a list of strings to display information about overdue books and their cardholders.
        /// </summary>
        /// <returns></returns>
        public List<string> GetOverdueBookInformation()
        {
            List<CheckOutLogBLL> overdues = logs.GetOverdueLogs();
            List<string> outputList = new List<string>();
            if (overdues.Count == 0)
            {
                outputList.Add("There are no records of overdue books.");
            }
            else
            {
                //I have logs, now I need book and cardholder
                overdues.Sort();
                foreach (CheckOutLogBLL l in overdues)
                {
                    BookBLL book = (from b in books
                                    where b.BookID == l.BookID
                                    select b).FirstOrDefault();
                    CardholderBLL cardholder = (CardholderBLL)(from c in people
                                                               where c is CardholderBLL
                                                               where c.ID == l.CardholderID
                                                               select c).FirstOrDefault();
                    //Build the string and add it to the listBox
                    string o = $"{book.ToString()} {l.ToString()} Cardholder: {cardholder.ToString()} ";
                    outputList.Add(o);
                }
            }
            return outputList;
        }
    }
}
