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
using BLLClassLibrary;

namespace WPFLibraryApp
{
    /// <summary>
    /// Interaction logic for AddRemoveCopiesWindow.xaml
    /// </summary>
    public partial class AddRemoveCopiesWindow : Window
    {
        BookUpdater updater;

        public AddRemoveCopiesWindow(BookUpdater Updater)
        {
            InitializeComponent();
            updater = Updater;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Reset();
                if (updater.RemoveCopies)
                {
                    this.Title = "Remove Copies";
                    instructionLabel.Content = "Please enter the number of copies to remove.";
                }
                else
                {
                    this.Title = "Add Copies";
                    instructionLabel.Content = "Please enter the number of copies to add.";
                }
                bookInfoLabel.Content = $"Current available copies: {updater.Book.NumberOfCopies}";
                bookTitleLabel.Content = $"{updater.Book.Title}";
            }
            catch(Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }

        void Reset()
        {
            errorLabel.Content = "";
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogResult = true;
            }
            catch (Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }
    }
}
