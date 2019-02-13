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
    /// Interaction logic for PatronSearchResultsPage.xaml
    /// </summary>
    public partial class SearchResultsPage : Page
    {
        PersonBLLCollection people;
        BookBLLCollection books;
        CheckOutLogBLLCollection logs;
        BookSearch searchResults;
        LibrarianBLL user;
        BookBLL selectedBook;
        List<string> listboxBookstrings;
        List<BookBLL> foundBooksFiltered;

        public SearchResultsPage(BookSearch SearchResults)
        {
            InitializeComponent();
            searchResults = SearchResults;
        }
        //This constructor is used to display the user (librarian) data of the logged in librarian.
        public SearchResultsPage(BookSearch SearchResults, LibrarianBLL User)
            : this(SearchResults)
        {
            user = User;
            modeLabel.Content = user.LoginMessage();
        }

        private void endButton_Click(object sender, RoutedEventArgs e)
        {

            this.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                bookDetailTextBox.Text = "";
                books = ((MainWindow)App.Current.MainWindow).Books;
                logs = ((MainWindow)App.Current.MainWindow).Logs;
                people = ((MainWindow)App.Current.MainWindow).People;
                //If in patron mode (no user is logged in) remove books with no copies
                if (user == null)
                {
                    foundBooksFiltered = new List<BookBLL>();
                    foreach (BookBLL b in searchResults.FoundBooks)
                    {
                        if (b.NumberOfCopies > 0)
                        {
                            foundBooksFiltered.Add(b);
                        }
                    }
                }
                else
                {
                    //Librarian Mode
                    foundBooksFiltered = searchResults.FoundBooks;
                }
                //Reports if no books were found.
                if (foundBooksFiltered.Count == 0)
                {
                    resultsLabel.Content = $"No books matched query: {searchResults.QueryTerms}";
                }
                else
                {
                    //Populates the listBox with the found books.
                    string resultsLabelString = $"{foundBooksFiltered.Count} book";
                    if (foundBooksFiltered.Count == 0 || foundBooksFiltered.Count > 1)
                    {
                        resultsLabelString += "s";
                    }
                    resultsLabelString += $" matched query: {searchResults.QueryTerms}";

                    resultsLabel.Content = resultsLabelString;
                    listboxBookstrings = new List<string>();
                    foreach (BookBLL b in foundBooksFiltered)
                    {
                        string infoString = AvailabilityBuilder(b);
                        listboxBookstrings.Add(infoString);
                    }
                    resultsListBox.DataContext = listboxBookstrings;
                }
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void resultsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Gets the selected book from the listbox, then finds the author
            int selectedIndex = resultsListBox.SelectedIndex;
            selectedBook = foundBooksFiltered[selectedIndex];
            PersonBLL selectedAuthor = people[selectedBook.AuthorID];

            //Finds the number of copies checked out for the book.
            int checkedOut = (from checkOut in logs
                              where checkOut.BookID == selectedBook.BookID
                              select checkOut).ToList().Count;

            //Display information about the selected book
            string output = selectedBook.Details(selectedAuthor, checkedOut);

            bookDetailTextBox.Text = output;
        }

        /// <summary>
        /// Uses the CheckOutLogCollection to modifty the strings of books that are not available.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        string AvailabilityBuilder(BookBLL b)
        {
            string output = "";
            if (logs.AvailableCopies(b) == 0)
            {
                output += "NOT AVAILABLE ";
            }
            output += b.ToString();
            return output;
        }



        private void bookDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (resultsListBox.SelectedIndex > -1)
            {
                //Just a handy window that stays up and tells you the details of the book you are looking for.
                BookDetailsWindow popup = new BookDetailsWindow(selectedBook.Title, bookDetailTextBox.Text);
                popup.Show();
            }
        }
    }
}
