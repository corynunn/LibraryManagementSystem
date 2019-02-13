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
    /// Interaction logic for StartingPage.xaml
    /// </summary>
    public partial class StartingPage : Page
    {
        PersonBLLCollection people;
        BookBLLCollection books;
        CheckOutLogBLLCollection logs;

        public StartingPage()
        {
            InitializeComponent();
        }

        void ResetFields()
        {
            errorLabel.Content = "";
            searchTextBox.Text = "";
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
                    ResetFields();

                    this.NavigationService.Navigate(new SearchResultsPage(newQuery));
                }
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LibrarianLoginWindow dialog = new LibrarianLoginWindow();
                if (dialog.ShowDialog() == true)
                {
                    ResetFields();
                    this.NavigationService.Navigate(new LibrarianMenu(dialog.User));
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

                ResetFields();
                greetingLabel.Content = "Greetings! You may search the catalog for books using key terms\nfrom the title, ISBN, subject, or author's name.";
                searchTextBox.Focus();
                books = ((MainWindow)App.Current.MainWindow).Books;
                logs = ((MainWindow)App.Current.MainWindow).Logs;
                people = ((MainWindow)App.Current.MainWindow).People;
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }
    }
}
