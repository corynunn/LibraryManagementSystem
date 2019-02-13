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
    /// Interaction logic for LibrarianMenu.xaml
    /// </summary>
    public partial class LibrarianMenu : Page
    {
        PersonBLLCollection people;
        BookBLLCollection books;
        CheckOutLogBLLCollection logs;

        LibrarianMenuLists librarianMenuLists;

        public LibrarianBLL User { get; private set; }

        public LibrarianMenu(LibrarianBLL librarian)
        {
            InitializeComponent();
            User = librarian;
        }

        /// <summary>
        /// Clears the input fields.
        /// </summary>
        void ResetFields()
        {
            errorLabel.Content = "";
            searchTextBox.Text = "";
            checkinTextBox.Text = "";
            outputListBox.DataContext = null;
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (searchTextBox.Text == "")
                {
                    errorLabel.Content = "Please enter terms to search for books, including ISBN,\nsubject, words from the title, or author's name.";
                    searchTextBox.Focus();
                }
                else
                {
                    string searchQuery = searchTextBox.Text.Trim();
                    BookSearch newQuery = new BookSearch(books, people, searchQuery);
                    newQuery.SearchAllFields();

                    this.NavigationService.Navigate(new SearchResultsPage(newQuery, User));
                    ResetFields();
                }
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void checkinAndOutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (checkinTextBox.Text == "")
                {
                    errorLabel.Content = "Please enter the patron's Library Card ID.";
                }
                else
                {
                    CardholderFinder cardholderFinder = new CardholderFinder(people, checkinTextBox.Text);

                    if (cardholderFinder.Cardholder == null)
                    {
                        errorLabel.Content = $"No cardholder was found matching given Library Card ID: {cardholderFinder.LibraryCardID}";
                    }
                    else
                    {
                        ResetFields();
                        this.NavigationService.Navigate(new CheckInAndOutMenu(User, cardholderFinder.Cardholder));
                    }
                }
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void updateCatalogButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.NavigationService.Navigate(new UpdateCatalogPage());
                
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
                ResetFields();
                books = ((MainWindow)App.Current.MainWindow).Books;
                logs = ((MainWindow)App.Current.MainWindow).Logs;
                people = ((MainWindow)App.Current.MainWindow).People;

                librarianMenuLists = new LibrarianMenuLists(people, books, logs);

                usernameLabel.Content = User.LoginMessage();
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }


        private void librarianListButton_Click(object sender, RoutedEventArgs e)
        {
            //displays librarian information
            try
            {
                outputListBox.DataContext = librarianMenuLists.GetLibrarians();
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void cardholdersListButton_Click(object sender, RoutedEventArgs e)
        {
            //creates a list of strings to show in listBox
            try
            {
                outputListBox.DataContext = librarianMenuLists.GetCardholderInformation();
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void authorListButton_Click(object sender, RoutedEventArgs e)
        {
            //Lists authors and their books
            try
            {
                outputListBox.DataContext = librarianMenuLists.GetAuthorInformation();
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void overdueBooks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Lists overdue books

                outputListBox.DataContext = librarianMenuLists.GetOverdueBookInformation();
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void addCardholderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.NavigationService.Navigate(new AddCardholderPage());
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }
    }
}
