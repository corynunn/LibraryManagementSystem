using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DatabaseFirstLibraryApp;
using BLLClassLibrary;

namespace WPFLibraryApp
{
    /// <summary>
    /// Interaction logic for CheckInAndOutMenu.xaml
    /// </summary>
    public partial class CheckInAndOutMenu : Page
    {

        PersonBLLCollection people;
        BookBLLCollection books;
        CheckOutLogBLLCollection logs;
        LibrarianBLL user;
        CardholderBLL patron;
        List<CheckOutLogBLL> borrowedBooks;
        List<CheckOutLogBLL> overDueLogs;
        bool goodStanding;

        public CheckInAndOutMenu(LibrarianBLL User, CardholderBLL Patron)
        {
            InitializeComponent();
            user = User;
            patron = Patron;
        }

        /// <summary>
        /// Reports the overdue books in the errorLabel.
        /// </summary>
        void ReportStanding()
        {
            string output = "Overdue book";
            if (overDueLogs.Count > 1)
            {
                output += "s";
            }
            output += ": ";

            foreach (CheckOutLogBLL l in overDueLogs)
            {
                output += l.Book.Title + ", ";
            }


            output += $"\n{patron.FirstName} {patron.LastName} may not check out any books till overdue books are returned.";
            errorLabel.Content = output;
        }

        /// <summary>
        /// Resets the ui fields.
        /// </summary>
        void ResetFields()
        {
            checkTextBox.Text = "";
            errorLabel.Content = "";
        }

        /// <summary>
        /// Sets the listBox to show the checked out books.
        /// </summary>
        void SetListBox()
        {
            ResetFields();
            borrowedBooks = GetCheckOutLogs();
            if (borrowedBooks.Count > 0)
            {
                overDueLogs = GetOverDueLogs();


                List<string> borrowedBooksStrings = new List<string>();
                foreach (CheckOutLogBLL l in borrowedBooks)
                {
                    BookBLL book = (from b in books
                                    where b.BookID == l.BookID
                                    select b).FirstOrDefault();

                    borrowedBooksStrings.Add($"{book.Title} {book.ISBN} " + l.ToString());
                }
                listBox.DataContext = borrowedBooksStrings;
            }
            else
            {
                string[] isEmpty = { "The patron has not currently checked anything out." };
                listBox.DataContext = isEmpty;
            }
        }

        List<CheckOutLogBLL> GetCheckOutLogs()
        {
            List<CheckOutLogBLL> borrowedBooks = new List<CheckOutLogBLL>();
            foreach (CheckOutLogBLL l in logs)
            {
                if (l.CardholderID == patron.ID)
                {
                    borrowedBooks.Add(l);
                }
            }
            return borrowedBooks;
        }

        List<CheckOutLogBLL> GetOverDueLogs()
        {
            List<CheckOutLogBLL> overDues = new List<CheckOutLogBLL>();

            foreach (CheckOutLogBLL l in borrowedBooks)
            {
                if ((DateTime.Now - l.CheckOutDate).TotalDays > 30)
                {
                    overDues.Add(l);
                }
            }
            return overDues;
        }

        /// <summary>
        /// Checks if the patron is in good standing or not, will report if bad.
        /// </summary>
        /// <returns></returns>
        bool IsInGoodStanding()
        {
            bool output = true;

            if (overDueLogs != null)
            {
                output = overDueLogs.Count == 0;
                if (!output)
                {
                    ReportStanding();
                }
            }

            return output;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void inButton_Click(object sender, RoutedEventArgs e)
        {
            //Returns a book, deleting the corresponding checkout log.
            try
            {
                //Check input
                if (checkTextBox.Text == "")
                {
                    errorLabel.Content = "Please enter the ISBN of the book the patron would like to return.";
                }
                else
                {
                    if (borrowedBooks.Count == 0)
                    {
                        errorLabel.Content = $"{patron.FirstName} {patron.LastName} doesn't have any books checked out";
                    }
                    else
                    {
                        //find book that matched the ISBN
                        BookBLL book = (from b in books
                                        where b.ISBN == checkTextBox.Text
                                        select b).FirstOrDefault();
                        if (book == null)
                        {
                            errorLabel.Content = $"No book found matching ISBN: {checkTextBox.Text}";
                        }
                        else
                        {
                            //Book was found, now check it against the checkoutlogs
                            bool bookAndLogFound = false;
                            foreach (CheckOutLogBLL l in borrowedBooks)
                            {
                                if (l.BookID == book.BookID)
                                {
                                    //Remove that log from context
                                    bookAndLogFound = true;
                                    logs.RemoveLogFromDatabase(l);
                                    SetListBox();
                                    break;
                                }

                            }
                            if (!bookAndLogFound)
                            {
                                errorLabel.Content = "User does not have that book checked out.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void outButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //First, check standing
                if (goodStanding)
                {
                    //Check input
                    if (checkTextBox.Text == "")
                    {
                        errorLabel.Content = "Please enter the ISBN of the book the patron would like to check out.";
                    }
                    else
                    {
                        BookBLL book = null;
                        foreach (BookBLL b in books)
                        {
                            //Find a matching book
                            if (b.ISBN == checkTextBox.Text)
                            {
                                book = b;
                                break;
                            }
                        }

                        if (book == null)
                        {
                            //No book was found matching the ISBN
                            errorLabel.Content = $"No book was found matching ISBN:{checkTextBox.Text}";
                        }
                        else
                        {
                            //A book was found, now check availability
                            if (logs.AvailableCopies(book) > 0)
                            {
                                //Now we can check the book out, creating a new log
                                CheckOutLogBLL newLog = new CheckOutLogBLL()
                                {
                                    CardholderID = patron.ID,
                                    BookID = book.BookID,
                                    CheckOutDate = DateTime.Now
                                };

                                //logs.Add(newLog);
                                logs.AddLogToDatabase(newLog);
                                SetListBox();
                            }
                            else
                            {
                                //There are no more available copies of that book
                                errorLabel.Content = $"All copies of {book.Title}: {book.ISBN} are checked out.";
                            }
                        }
                    }
                }
                else
                {
                    ReportStanding();
                }
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //Set up screen
                ResetFields();
                usernameLabel.Content = user.LoginMessage();
                patronLabel.Content = "Patron: " + patron.ToString();

                //Set fields
                books = ((MainWindow)App.Current.MainWindow).Books;
                logs = ((MainWindow)App.Current.MainWindow).Logs;
                people = ((MainWindow)App.Current.MainWindow).People;


                //Get list of checked out logs and the list of overdue logs
                SetListBox();


                //Check for good standing, report if bad
                goodStanding = IsInGoodStanding();
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }
    }
}
