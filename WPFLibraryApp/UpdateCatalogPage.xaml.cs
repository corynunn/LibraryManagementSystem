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
using BLLClassLibrary;

namespace WPFLibraryApp
{
    /// <summary>
    /// Interaction logic for UpdateCatalogPage.xaml
    /// </summary>
    public partial class UpdateCatalogPage : Page
    {
        BookBLLCollection books;
        PersonBLLCollection people;
        CheckOutLogBLLCollection logs;
        List<BookBLL> bookList;

        public UpdateCatalogPage()
        {
            InitializeComponent();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //Sets up the collection references and sets the page
                books = ((MainWindow)App.Current.MainWindow).Books;
                logs = ((MainWindow)App.Current.MainWindow).Logs;
                people = ((MainWindow)App.Current.MainWindow).People;

                Reset();
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Resets the screen and fields.
        /// </summary>
        void Reset()
        {
            errorLabel.Content = "";
            searchTextBox.Text = "";
            iSBNTextBox.Text = "";
            bookList = books.GetBooks();
            booksListBox.DataContext = bookList;
            booksListBox.SelectedIndex = -1;
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Searchs for books and lists matches below to aid updating
                BookSearch search = new BookSearch(books, people, searchTextBox.Text.Trim());
                search.SearchAllFields();
                booksListBox.DataContext = search.FoundBooks;
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }


        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BookSearch search = new BookSearch(books, people, iSBNTextBox.Text.Trim());
                search.ISBNSearch();
                if (search.FoundBooks.Count == 0)
                {
                    throw new Exception("No book was found matching that ISBN");
                }
                BookUpdater updater = new BookUpdater(books, people,logs, search.FoundBooks[0], true);
                Reset();
                this.NavigationService.Navigate(new AddSubtractCopiesPage(updater));
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Searches for a book by ISBN, if one is found then additional copies will be added to it.
                //If no book is found then a new book will be created.
                if (iSBNTextBox.Text == "")
                {
                    throw new Exception("Please enter an ISBN");
                }
                BookSearch search = new BookSearch(books, people, iSBNTextBox.Text.Trim());
                search.ISBNSearch();
                if (search.FoundBooks.Count == 0)
                {
                    //A new book is being added to the database
                    //Create the bookBuilder that will make the bookBLL
                    BookBuilder builder = new BookBuilder(books, people, iSBNTextBox.Text.Trim());
                    //Request author information
                    RequestAuthorWindow dialog1 = new RequestAuthorWindow(builder);
                    if (dialog1.ShowDialog() == true)
                    {
                        //Check if a new author is being created, is so ask for bio
                        if (builder.AuthorBuilder != null)
                        {
                            RequestAuthorBio dialog2 = new RequestAuthorBio(builder);
                            dialog2.ShowDialog();
                        }
                        Reset();
                        this.NavigationService.Navigate(new NewBookKnownAuthor(builder));
                    }
                    
                }
                else
                {
                    BookUpdater updater = new BookUpdater(books, people, logs, search.FoundBooks[0]);
                    Reset();
                    this.NavigationService.Navigate(new AddSubtractCopiesPage(updater));
                }
            }
            catch (Exception ex)
            {

                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void booksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (booksListBox.SelectedIndex != -1)
                {
                    BookBLL book = (BookBLL)booksListBox.SelectedItem;
                    iSBNTextBox.Text = book.ISBN;
                }
            }
            catch (Exception ex)
            {

                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (iSBNTextBox.Text == "")
                {
                    throw new Exception("Please enter the ISBN of the book you would like to update.");
                }
                BookSearch search = new BookSearch(books, people, iSBNTextBox.Text.Trim());
                search.ISBNSearch();
                if (search.FoundBooks.Count == 0)
                {
                    throw new Exception("No book was found with that ISBN.");
                }
                //The found book will now be placed into a bookUpdater object for the updating process
                BookUpdater updater = new BookUpdater(books, people, logs, search.FoundBooks[0]);
                //Load the bookUpdatePage
                Reset();
                this.NavigationService.Navigate(new UpdateBookPage(updater));
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }
    }
}
