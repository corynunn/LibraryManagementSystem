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
    /// Interaction logic for LibrarianLoginWindow.xaml
    /// </summary>
    public partial class LibrarianLoginWindow : Window
    {
        PersonBLLCollection people;

        public LibrarianBLL User { get; private set; }

        public LibrarianLoginWindow()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (usernameTextBox.Text == "")
                {
                    errorLabel.Content = "Please enter your Librarian username.";
                }
                else if (passwordTextBox.Password == "")
                {
                    errorLabel.Content = "Please enter your password.";
                }
                else
                {
                    LibrarianBLL user = null;

                    foreach (PersonBLL p in people)
                    {
                        if (p is LibrarianBLL l)
                        {
                            if (usernameTextBox.Text == l.UserID)
                            {
                                if (passwordTextBox.Password == l.Password)
                                {
                                    User = l;
                                    DialogResult = true;
                                }
                            }
                        }

                        if (user == null)
                        {
                            errorLabel.Content = "Invalid username and password.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                errorLabel.Content = "";
                usernameTextBox.Focus();
                people = ((MainWindow)App.Current.MainWindow).People;
            }
            catch(Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }
    }
}
