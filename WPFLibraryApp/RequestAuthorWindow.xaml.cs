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
using System.Windows.Shapes;
using DatabaseFirstLibraryApp;
using BLLClassLibrary;

namespace WPFLibraryApp
{
    /// <summary>
    /// Interaction logic for RequestAuthorWindow.xaml
    /// </summary>
    public partial class RequestAuthorWindow : Window
    {
        //Properties
        public BookBuilder Builder { get; private set; }
        

        public RequestAuthorWindow(BookBuilder NewBuilder)
        {
            InitializeComponent();
            
            Builder = NewBuilder;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            windowLabel1.Content = $"{Builder.Book.ISBN} is not in the catalog, to";
            windowLabel2.Content = "enter it provide the author's names.";
            errorLabel.Content = "";
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            //Searches the database for the entered first and last names.
            //Three results are possible
            //1. The author is already in the database, we just need to add the book
            //2. The person is in the database, but not an author, we will add an author to the database
            //3. Completely new person, a person will be added to the people table and an author will be added to the author table.
            //Using ToList I'll create a people list with 0, 1, or 2 count
            try
            {
                string firstName = firstNameTextBox.Text.Trim();
                string lastName = lastNameTextBox.Text.Trim();
                if (firstName == "" || lastName == "")
                {
                    errorLabel.Content = "Please enter the author's first and last names.";
                }
                else
                {
                    Builder.CheckNames(firstName, lastName);
                    DialogResult = true;

                }
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            //Cancels the operation
            DialogResult = false;
        }
    }
}
