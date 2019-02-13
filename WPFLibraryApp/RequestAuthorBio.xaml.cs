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
    /// Interaction logic for RequestAuthorBio.xaml
    /// </summary>
    public partial class RequestAuthorBio : Window
    {
        IAuthorBuilderable builder;

        public RequestAuthorBio(IAuthorBuilderable Builder)
        {
            InitializeComponent();
            builder = Builder;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bioTextBox.Text.Trim() != "")
                {
                    builder.SetBio(bioTextBox.Text.Trim());
                }
                DialogResult = true;
            }
            catch(Exception ex)
            {
                errorLabel.Content = ErrorHandler.InnermostExceptionMessage(ex);
            }
        }
    }
}
