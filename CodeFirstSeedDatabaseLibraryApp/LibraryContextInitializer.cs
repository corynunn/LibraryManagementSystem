using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Xml.Linq;
using System.IO;

namespace CodeFirstSeedDatabaseLibraryApp
{
    public class LibraryContextInitializer: DropCreateDatabaseIfModelChanges<LibraryContextCodeFirst>
    {
        protected override void Seed(LibraryContextCodeFirst context)
        {
            //Load order (to set up database properly):
            //People
            //Librarians
            //Authors
            //Cardholders
            //Books
            //CheckOutLogs

            XMLListRetriever lists = new XMLListRetriever();

            //Load People from the XML file and add them to the context
            string path = XMLFileFinder.FindPath("People.xml");
            lists.GetPersonCodeFirstsFromXDocument(path);
            context.People.AddRange(lists.People);
            context.SaveChanges();

            //Repeat with Librarians
            path = XMLFileFinder.FindPath("Librarians.xml");
            lists.GetLibrarianCodeFirstsFromXDocument(path);
            context.Librarians.AddRange(lists.Librarians);
            context.SaveChanges();

            //Repeat with Authors
            path = XMLFileFinder.FindPath("Authors.xml");
            lists.GetAuthorCodeFirstsFromXDocument(path);
            context.Authors.AddRange(lists.Authors);
            context.SaveChanges();

            //Repeat with Cardholder
            path = XMLFileFinder.FindPath("Cardholders.xml");
            lists.GetCardholderCodeFirstsFromXDocument(path);
            context.Cardholders.AddRange(lists.Cardholders);
            context.SaveChanges();

            //Repeat with Books
            path = XMLFileFinder.FindPath("Books.xml");
            lists.GetBookCodeFirstsFromXDocument(path);
            context.Books.AddRange(lists.Books);
            context.SaveChanges();

            //Repeat with CheckOutLogs
            path = XMLFileFinder.FindPath("CheckOutLog.xml");
            lists.GetCheckOutLogCodeFirstsFromXDocument(path);
            context.CheckOutLogs.AddRange(lists.Logs);
            context.SaveChanges();

        }

        
        
    }

    
}
