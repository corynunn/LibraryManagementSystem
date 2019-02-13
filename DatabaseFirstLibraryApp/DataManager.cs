using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstSeedDatabaseLibraryApp;

namespace DatabaseFirstLibraryApp
{
    //This class handles interactions with the context so we aren't leaving an open connection to database sitting around.
    //It will also be called at the close of the application to serialize the database into xml files.
    public class DataManager: IDisposable
    {
        public string PeopleXMLPath { get; private set; }
        public string AuthorsXMLPath { get; private set; }
        public string CardholdersXMLPath { get; private set; }
        public string LibrariansXMLPath { get; private set; }
        public string CheckOutLogsXMLPath { get; private set; }
        public string BooksXMLPath { get; private set; }

        //The constructor will find the paths of the 6 XML files for serialization at Close.
        public DataManager()
        {
            PeopleXMLPath = XMLFileFinder.FindPath("People.xml");
            AuthorsXMLPath = XMLFileFinder.FindPath("Authors.xml");
            CardholdersXMLPath = XMLFileFinder.FindPath("Cardholders.xml");
            LibrariansXMLPath = XMLFileFinder.FindPath("Librarians.xml");
            CheckOutLogsXMLPath = XMLFileFinder.FindPath("CheckOutLog.xml");
            BooksXMLPath = XMLFileFinder.FindPath("Books.xml");
        }

        

        #region IDisposable Support
        private bool disposedValue = false; //To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    using (LibraryDBEntities context = new LibraryDBEntities())
                    {
                        List<Person> people = context.People.ToList();
                        List<Author> authors = context.Authors.ToList();
                        List<Cardholder> cardholders = context.Cardholders.ToList();
                        List<Librarian> librarians = context.Librarians.ToList();
                        List<CheckOutLog> logs = context.CheckOutLogs.ToList();
                        List<Book> books = context.Books.ToList();

                        XMLSerializer.SerializePeople(people, PeopleXMLPath);
                        XMLSerializer.SerializeAuthors(authors, AuthorsXMLPath);
                        XMLSerializer.SerializeCardholders(cardholders, CardholdersXMLPath);
                        XMLSerializer.SerializeLibrarians(librarians, LibrariansXMLPath);
                        XMLSerializer.SerializeCheckOutLogs(logs, CheckOutLogsXMLPath);
                        XMLSerializer.SerializeBooks(books, BooksXMLPath);
                    }
                }
                disposedValue = true;
            }
        }

        //This code is needed to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
        
    }
}
