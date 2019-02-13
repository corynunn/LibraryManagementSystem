using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatabaseFirstLibraryApp
{
    public static class XMLSerializer
    {
        //This static class just holds the methods for serializing the six tables into XML files.
        public static void SerializePeople(List<Person> People, string path)
        {
            //A new list is needed here due to LINQ requiring it
            List<Person> people = (from p in People
                                   orderby p.PersonID
                                   select p).ToList();

            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("Contents of People table in database."),
                    new XElement("People",
                        from p in people
                        select new XElement("Person",
                               new XElement("PersonID", p.PersonID),
                               new XElement("FirstName", p.FirstName),
                               new XElement("LastName", p.LastName))));

            document.Save(path);
        }
        public static void SerializeAuthors(List<Author> Authors, string path)
        {
            List<Author> authors = (from a in Authors
                                    orderby a.ID
                                    select a).ToList();

            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("Contents of Authors table in database."),
                    new XElement("Authors",
                        from a in authors
                        select new XElement("Author",
                               new XElement("ID", a.ID),
                               a.Bio == null ? null :
                               new XElement("Bio", a.Bio))));

            document.Save(path);
        }
        public static void SerializeCardholders(List<Cardholder> Cardholders, string path)
        {
            List<Cardholder> cardholders = (from c in Cardholders
                                   orderby c.ID
                                   select c).ToList();

            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("Contents of Cardholders table in database."),
                    new XElement("Cardholders",
                        from c in cardholders
                        select new XElement("Cardholder",
                               new XElement("ID", c.ID),
                               new XElement("Phone", c.Phone),
                               new XElement("LibraryCardID", c.LibraryCardID))));

            document.Save(path);
        }
        public static void SerializeLibrarians(List<Librarian> Librarians, string path)
        {
            List<Librarian> librarians = (from l in Librarians
                                   orderby l.ID
                                   select l).ToList();

            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("Contents of Librarians table in database."),
                    new XElement("Librarians",
                        from l in librarians
                        select new XElement("Librarian",
                               new XElement("ID", l.ID),
                               new XElement("Phone", l.Phone),
                               new XElement("UserID", l.UserID),
                               new XElement("Password", l.Password))));

            document.Save(path);
        }
        public static void SerializeCheckOutLogs(List<CheckOutLog> CheckOutLogs, string path)
        {
            List<CheckOutLog> checkOutLogs = (from c in CheckOutLogs
                                   orderby c.CheckOutLogID
                                   select c).ToList();

            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("Contents of CheckOutLog table in database."),
                    new XElement("CheckOutLogs",
                        from c in checkOutLogs
                        select new XElement("CheckOutLog",
                               new XElement("CheckOutLogID", c.CheckOutLogID),
                               new XElement("CardholderID", c.CardholderID),
                               new XElement("BookID", c.BookID),
                               new XElement("CheckOutDate", c.CheckOutDate))));

            document.Save(path);
        }
        public static void SerializeBooks(List<Book> Books, string path)
        {
            List<Book> books = (from b in Books
                                   orderby b.BookID
                                   select b).ToList();

            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("Contents of Books table in database."),
                    new XElement("Books",
                        from b in books
                        select new XElement("Book",
                               new XElement("BookID", b.BookID),
                               new XElement("ISBN", b.ISBN),
                               new XElement("Title", b.Title),
                               new XElement("AuthorID", b.AuthorID),
                               b.NumPages == null ? null :
                               new XElement("NumPages", b.NumPages),
                               b.Subject == null ? null :
                               new XElement("Subject", b.Subject),
                               b.Description == null ? null :
                               new XElement("Description", b.Description),
                               b.Publisher == null ? null :
                               new XElement("Publisher", b.Publisher),
                               b.YearPublished == null ? null :
                               new XElement("YearPublished", b.YearPublished),
                               b.Language == null ? null :
                               new XElement("Language", b.Language),
                               new XElement("NumberOfCopies", b.NumberOfCopies))));

            document.Save(path);
        }
    }
}
