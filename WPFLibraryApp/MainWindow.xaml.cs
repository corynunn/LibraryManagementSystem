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
using CodeFirstSeedDatabaseLibraryApp;
using DatabaseFirstLibraryApp;
using BLLClassLibrary;

namespace WPFLibraryApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Declare variables used throughout the mainwindow
        DataManager dataManager;

        //New BLL stuff
        public BookBLLCollection Books { get; private set; }
        public PersonBLLCollection People { get; private set; }
        public CheckOutLogBLLCollection Logs { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //If no database exists this will create it with Code First, otherwise this does nothing.
            CreateCodeFirstDatabase.CreateAndInitializeDatabase();

            //The datamanger will handle locating and creating the XML files when the program ends.
            dataManager = new DataManager();

            //Populate the three collections (books, people, checkoutlogs)
            Books = new BookBLLCollection();
            Books.PopulateBooks();
            People = new PersonBLLCollection();
            People.PopulatePeople();
            Logs = new CheckOutLogBLLCollection();
            Logs.PopulateLogs();

            Main.Content = new StartingPage();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            dataManager.Dispose();
        }
    }
}
