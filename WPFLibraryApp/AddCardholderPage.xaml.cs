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
    /// Interaction logic for AddCardholderPage.xaml
    /// </summary>
    public partial class AddCardholderPage : Page
    {
        PersonBLLCollection people;

        public AddCardholderPage()
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
                people = ((MainWindow)App.Current.MainWindow).People;
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
                if (firstNameTextBox.Text == "")
                {
                    errorLabel.Content = "Please enter a First Name.";
                }
                else if (lastNameTextBox.Text == "")
                {
                    errorLabel.Content = "Please enter a Last Name.";
                }
                else if (phoneTextBox.Text == "")
                {
                    errorLabel.Content = "Please enter a Phone Number.";
                }
                else if (libraryCardIDTextBox.Text == "")
                {
                    errorLabel.Content = "Please enter a Library Card ID.";
                }
                else
                {
                    CardholderBuilder cardholder = new CardholderBuilder(people); //firstNameTextBox.Text, lastNameTextBox.Text, phoneTextBox.Text, libraryCardIDTextBox.Text);
                    if (!cardholder.CheckNames(firstNameTextBox.Text.Trim(), lastNameTextBox.Text.Trim()))
                    {
                        throw new Exception("Cardholder with that name already exists!");
                    }
                    if (!cardholder.CheckLibraryCardID(libraryCardIDTextBox.Text.Trim()))
                    {
                        throw new Exception("Library Card ID is invalid (either already in use or is too long.");
                    }
                    if (!cardholder.CheckPhoneNumber(phoneTextBox.Text.Trim()))
                    {
                        throw new Exception("Phone number is too long (20 characters max)");
                    }

                    cardholder.CreateCardholder();
                    people.AddCardholderToDatabase(cardholder);
                    errorLabel.Content = $"{cardholder.Cardholder.FirstName} {cardholder.Cardholder.LastName} was added to the database.";
                }
            }
            catch(Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }
    }
}
