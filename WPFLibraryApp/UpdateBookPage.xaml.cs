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
    /// Interaction logic for UpdateBookPage.xaml
    /// </summary>
    public partial class UpdateBookPage : Page
    {
        BookUpdater updater;
        BookBLLCollection books;

        public UpdateBookPage(BookUpdater Updater)
        {
            InitializeComponent();
            updater = Updater;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                books = ((MainWindow)App.Current.MainWindow).Books;
                Reset();
            }
            catch (Exception ex)
            {

                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Gather and trim the inputs
                string firstName = firstNameTextBox.Text.Trim();
                string lastName = lastNameTextBox.Text.Trim();
                string iSBN = iSBNTextBox.Text.Trim();
                string title = titleTextBox.Text.Trim();
                int numberOfCopies;
                if (!int.TryParse(copiesTextBox.Text,out numberOfCopies) || numberOfCopies < 0)
                {
                    throw new Exception("Failed to parse the number of copies. Must enter a number, 0 or more.");
                }
                
                //Since numPages is a nullable int some more logic is required for formating, and parsing.
                int? numPages = null;
                if (numPagestextBox.Text != "")
                {
                    if (!int.TryParse(numPagestextBox.Text, out int numPagesint))
                    {
                        throw new Exception("If entering a page count please enter a valid number.");
                    }
                    else
                    {
                        //A valid number was entered, set the pages.
                        if (numPagesint < 1)
                        {
                            throw new Exception("Pages should be 1 or more.");
                        }
                        numPages = numPagesint;
                    }
                }
                string subject = subjectTextBox.Text.Trim();
                string description = descriptionTextBox.Text.Trim();
                string publisher = publisherTextBox.Text.Trim();
                if (publisher.Count() > 50)
                {
                    throw new Exception("Publisher name cannot be greater than 50 characters.");
                }
                string yearPublished = yearTextBox.Text.Trim();
                if (yearPublished != "")
                {
                    if (yearPublished.Count() != 4)
                    {
                        throw new Exception("If entering a year it must be 4 characters");
                    }
                }
                string language = languageTextBox.Text.Trim();

                //Check the required fields to make sure there is something in them all
                if (firstName == "" || lastName == "" || title == "" || iSBN == "")
                {
                    throw new Exception("Required fields cannot be left blank.");
                }
                if (titleTextBox.Text.Trim().Count() > 50)
                {
                    throw new Exception("Title is too long, 50 char maximum.");
                }
                if (iSBN.Count() > 50)
                {
                    throw new Exception("ISBN is too long, 50 character maximum");
                }
                //*******************
                //ISBN, Author, and Copies logic is not complete yet
                //*******************

                //The values can now be sent to the BookUpdater, it will handle the logic
                //Required Fields
                updater.UpdateISBN(iSBN);
                updater.UpdateTitle(title);
                updater.UpdateNumberOfCopies(numberOfCopies);
                updater.UpdateAuthorID(firstName, lastName);//this might need to be turned off for testing
                //If a new author object is being created a bio needs to be requested
                if (updater.AuthorBuilder != null)
                {
                    RequestAuthorBio dialog = new RequestAuthorBio(updater);
                    dialog.ShowDialog();
                }

                //Optional Fields                
                updater.UpdateNumPages(numPages);
                updater.UpdateSubject(subject);
                updater.UpdateDescription(description);
                updater.UpdatePublisher(publisher);
                updater.UpdateYearPub(yearPublished);
                updater.UpdateLanguage(language);

                //Update the book record in the database
                books.UpdateBookFields(updater);
                this.NavigationService.GoBack();
            }
            catch (Exception ex)
            {

                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
                updater.Reset();
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Reset();
                updater.Reset();
            }
            catch (Exception ex)
            {

                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        /// <summary>
        /// Resets the fields on the screen to the starting values.
        /// </summary>
        void Reset()
        {
            errorLabel.Content = "";
            firstNameTextBox.Text = updater.FirstName;
            lastNameTextBox.Text = updater.LastName;
            iSBNTextBox.Text = updater.Book.ISBN;
            titleTextBox.Text = updater.Book.Title;
            copiesTextBox.Text = updater.Book.NumberOfCopies.ToString();
            numPagestextBox.Text = updater.Book.NumPages.ToString();
            subjectTextBox.Text = updater.Book.Subject;
            descriptionTextBox.Text = updater.Book.Description;
            publisherTextBox.Text = updater.Book.Publisher;
            yearTextBox.Text = updater.Book.YearPublished;
            languageTextBox.Text = updater.Book.Language;
        }
    }
}
