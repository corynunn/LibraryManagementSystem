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
    /// Interaction logic for NewBookKnownAuthor.xaml
    /// </summary>
    public partial class NewBookKnownAuthor : Page
    {
        BookBuilder builder;
        BookBLLCollection books;
        PersonBLLCollection people;

        public NewBookKnownAuthor(BookBuilder Builder)
        {
            InitializeComponent();
            builder = Builder;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            //This cancels the operation, nothing is saved
            this.NavigationService.GoBack();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int copies;
                int.TryParse(numPagestextBox.Text, out int pages);
                if (titleTextBox.Text == "" || !int.TryParse(copiesTextBox.Text, out copies))
                {
                    errorLabel.Content = "Please enter valid data into the required fields.";
                }
                else if(copies < 0)
                {
                    errorLabel.Content = "Please enter a value of 0 or more copies of the book.";
                }
                else if(numPagestextBox.Text != "" && pages < 1)
                {
                    errorLabel.Content = "If entering page count please enter a value of 1 or more.";
                }
                else if(yearTextBox.Text != "" && yearTextBox.Text.Trim().Count() != 4)
                {
                    errorLabel.Content = "If entering a year it must be in a 4 char format.";
                }
                else
                {
                    
                    //Fill in the required data
                    builder.SetTitle(titleTextBox.Text.Trim());
                    builder.SetCopies(copies);

                    //Fill in the optional fields
                    if (numPagestextBox.Text != "")
                    {
                        builder.SetPages(pages);
                    }
                    if (subjectTextBox.Text != "")
                    {
                        builder.SetSubject(subjectTextBox.Text.Trim());
                    }
                    if (descriptionTextBox.Text != "")
                    {
                        builder.SetDescription(descriptionTextBox.Text.Trim());
                    }
                    if (publisherTextBox.Text != "")
                    {
                        builder.SetPublisher(publisherTextBox.Text.Trim());
                    }
                    if (yearTextBox.Text != "")
                    {
                        builder.SetYear(yearTextBox.Text);
                    }
                    if (languageTextBox.Text != "")
                    {
                        builder.SetLanguage(languageTextBox.Text);
                    }
                    //With all fields added create the book
                    //builder.CreateBook();
                    //Save the new book to the database.
                    books.AddBookToDatabase(builder);
                    //Return to Librarian Menu
                    this.NavigationService.GoBack();
                }
            }
            catch(Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                errorLabel.Content = "";
                books = ((MainWindow)App.Current.MainWindow).Books;
                people = ((MainWindow)App.Current.MainWindow).People;

                //Set the author detail box

                if(builder.AuthorBuilder == null)
                {
                    //There is already in author, load the text with the info
                    List<PersonBLL> personBLLs = (from x in people
                                                 where x.ID == builder.Book.AuthorID
                                                 select x).ToList();
                    foreach (PersonBLL p in personBLLs)
                    {
                        if (p is AuthorBLL a)
                        {
                            authorDetailTextBox.Text = a.ToString();
                        }
                    }
                }
                else
                {
                    //A new author will be created at the end of the process, just report their first and last name
                    string output = $"{builder.AuthorBuilder.Author.FirstName} {builder.AuthorBuilder.Author.LastName}";
                    authorDetailTextBox.Text = output;
                }

                //Set the given ISBN
                givenISBN.Content = builder.Book.ISBN;
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }
    }
}
