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
    /// Interaction logic for AddSubtractCopiesPage.xaml
    /// </summary>
    public partial class AddSubtractCopiesPage : Page
    {
        PersonBLLCollection people;
        BookBLLCollection books;
        CheckOutLogBLLCollection logs;

        BookUpdater updater;

        public AddSubtractCopiesPage(BookUpdater Updater)
        {
            InitializeComponent();
            updater = Updater;
        }

        /// <summary>
        /// Provides detailed information on the book being edited.
        /// </summary>
        void GetBookLongDetails()
        {
            //Finds author of book
            PersonBLL selectedAuthor = (from p in people
                                     where p.ID == updater.Book.AuthorID
                                     select p).FirstOrDefault();
            //Finds the number of copies checked out for the book.
            int checkedOut = (from checkOut in logs
                              where checkOut.BookID == updater.Book.BookID
                              select checkOut).ToList().Count;

            //Display information about the selected book
            string output = updater.Book.Details(selectedAuthor, checkedOut);

            bookDetailTextBox.Text = output;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        //private void addButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        AlterCopiesNegatively(false);
        //    }
        //    catch (Exception ex)
        //    {
        //        errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
        //    }
        //}

        //void AlterCopiesNegatively(bool subtract)
        //{
        //    int copies = 0;
        //    if (!int.TryParse(updateCatalogTextBox.Text, out copies) || copies < 1)
        //    {
        //        errorLabel.Content = "Please enter a valid number greater than 0.";
        //    }
        //    else
        //    {
        //        if (subtract)
        //        {
        //            int availableCopies = logs.AvailableCopies(book);
        //            if (copies > availableCopies)
        //            {
        //                throw new Exception("You can't subtract that many copies.");
        //            }
        //            copies *= -1;
        //        }
        //        books.ChangeCopiesNumber(book.BookID, copies);
        //        book = (from b in books
        //                where b.BookID == book.BookID
        //                select b).FirstOrDefault();
        //        GetBookLongDetails();
        //    }
        //}

        //private void subtractButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        AlterCopiesNegatively(true);
        //    }
        //    catch (Exception ex)
        //    {
        //        errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
        //    }
        //}

        /// <summary>
        /// Resets the error label, textBox, and the book details.
        /// </summary>
        void Reset()
        {
            errorLabel.Content = "";
            updateCatalogTextBox.Text = "";
            GetBookLongDetails();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                errorLabel.Content = "";
                books = ((MainWindow)App.Current.MainWindow).Books;
                logs = ((MainWindow)App.Current.MainWindow).Logs;
                people = ((MainWindow)App.Current.MainWindow).People;
                if (updater.RemoveCopies == true)
                {
                    instructionsLabel.Content = "You may remove copies from the selected book.";
                }

                GetBookLongDetails();
            }
            catch(Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
            
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int x;
                if (!int.TryParse(updateCatalogTextBox.Text, out x))
                {
                    throw new Exception("Please enter a value number to update the record.");
                    
                }
                updater.AddOrSubtractCopies(x);
                Reset();
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }
    }
}
