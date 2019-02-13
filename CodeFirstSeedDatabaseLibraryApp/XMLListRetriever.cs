using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeFirstSeedDatabaseLibraryApp
{
    public class XMLListRetriever
    {
        //Properties for the lists
        public List<AuthorCodeFirst> Authors { get; private set; }
        public List<LibrarianCodeFirst> Librarians { get; private set; }
        public List<CardholderCodeFirst> Cardholders { get; private set; }
        public List<PersonCodeFirst> People { get; private set; }
        public List<CheckOutLogCodeFirst> Logs { get; private set; }
        public List<BookCodeFirst> Books { get; private set; }

        /// <summary>
        /// Sets the property of AuthorCodeFirst objects from an XML file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public void GetAuthorCodeFirstsFromXDocument(string path)
        {
            //Get XElements using LINQ
            var authorsXML = (from a in XDocument.Load(path).Descendants("Author")
                              select a).ToList();

            List<AuthorCodeFirst> authors = new List<AuthorCodeFirst>(authorsXML.Count);
            AuthorCodeFirst author;

            //Loops through each XElement of "Author"
            foreach (XElement a in authorsXML)
            {
                author = new AuthorCodeFirst();
                //Loops through the child XElements
                foreach (XElement x in a.Elements())
                {
                    //Switch loop is needed because of nullable fields
                    switch (x.Name.ToString())
                    {
                        case "ID":
                            author.ID = int.Parse(a.Element("ID").Value.Trim());
                            break;
                        case "Bio":
                            author.Bio = a.Element("Bio").Value.Trim();
                            break;
                    }
                }
                authors.Add(author);
            }
            Authors = authors;
        }
        /// <summary>
        /// Sets the property of BookCodeFirst objects from an XML file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public void GetBookCodeFirstsFromXDocument(string path)
        {
            //Get xelements using LINQ
            var booksXML = (from b in XDocument.Load(path).Descendants("Book")
                            select b).ToList();
            List<BookCodeFirst> books = new List<BookCodeFirst>(booksXML.Count);
            BookCodeFirst book;

            //Loop through each book entry in the XML file
            foreach (XElement b in booksXML)
            {
                book = new BookCodeFirst();
                //Loop through the child XElements in each book
                foreach (XElement x in b.Elements())
                {
                    //Swith is needed for the non-required fields
                    switch (x.Name.ToString())
                    {
                        case "BookID":
                            book.BookID = int.Parse(b.Element("BookID").Value.Trim());
                            break;
                        case "ISBN":
                            book.ISBN = b.Element("ISBN").Value.Trim();
                            break;
                        case "Title":
                            book.Title = b.Element("Title").Value.Trim();
                            break;
                        case "AuthorID":
                            book.AuthorID = int.Parse(b.Element("AuthorID").Value.Trim());
                            break;
                        case "NumPages":
                            book.NumPages = int.Parse(b.Element("NumPages").Value.Trim());
                            break;
                        case "Subject":
                            book.Subject = b.Element("Subject").Value.Trim();
                            break;
                        case "Description":
                            book.Description = b.Element("Description").Value.Trim();
                            break;
                        case "Publisher":
                            book.Publisher = b.Element("Publisher").Value.Trim();
                            break;
                        case "YearPublished":
                            book.YearPublished = b.Element("YearPublished").Value.Trim();
                            break;
                        case "Language":
                            book.Language = b.Element("Language").Value.Trim();
                            break;
                        case "NumberOfCopies":
                            book.NumberOfCopies = int.Parse(b.Element("NumberOfCopies").Value.Trim());
                            break;
                    }
                }
                books.Add(book);
            }
            Books = books;
        }
        /// <summary>
        /// Sets the property of CardholderCodFirst from an XML file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public void GetCardholderCodeFirstsFromXDocument(string path)
        {
            //Get a List<XElements> using LINQ
            var cardholdersXML = (from c in XDocument.Load(path).Descendants("Cardholder")
                                  select c).ToList();
            //List to store the cardholders
            List<CardholderCodeFirst> cardholders = new List<CardholderCodeFirst>(cardholdersXML.Count);
            CardholderCodeFirst cardholder;

            foreach (var c in cardholdersXML)
            {
                cardholder = new CardholderCodeFirst()
                {
                    ID = int.Parse(c.Element("ID").Value.Trim()),
                    Phone = c.Element("Phone").Value.Trim(),
                    LibraryCardID = c.Element("LibraryCardID").Value.Trim()
                };
                cardholders.Add(cardholder);
            }
            Cardholders = cardholders;
        }
        /// <summary>
        /// Sets the property of CheckOutLogCodeFirst objects from an XML file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public void GetCheckOutLogCodeFirstsFromXDocument(string path)
        {
            //Get the data from the XML file via LINQ
            List<XElement> logsXML = (from l in XDocument.Load(path).Descendants("CheckOutLog")
                                      select l).ToList();
            //List to store the logs
            List<CheckOutLogCodeFirst> logs = new List<CheckOutLogCodeFirst>(logsXML.Count);
            CheckOutLogCodeFirst log;

            //loop through the XElements creating new logs
            foreach (var l in logsXML)
            {
                log = new CheckOutLogCodeFirst()
                {
                    CheckOutLogID = int.Parse(l.Element("CheckOutLogID").Value.Trim()),
                    CardholderID = int.Parse(l.Element("CardholderID").Value.Trim()),
                    BookID = int.Parse(l.Element("BookID").Value.Trim()),
                    CheckOutDate = DateTime.Parse(l.Element("CheckOutDate").Value.Trim())
                };
                logs.Add(log);
            }
            Logs = logs;
        }
        /// <summary>
        /// Sets the property of LibrarianCodeFirst objects from an XML file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public void GetLibrarianCodeFirstsFromXDocument(string path)
        {
            //Get XElements using LINQ
            List<XElement> librariansXML = (from l in XDocument.Load(path).Descendants("Librarian")
                                            select l).ToList();

            //Collection to store the new librarian objects
            List<LibrarianCodeFirst> librarians = new List<LibrarianCodeFirst>(librariansXML.Count);

            LibrarianCodeFirst librarian;

            //Loop through the XElement objects and instantiate a new person and add them to the collection
            foreach (XElement l in librariansXML)
            {
                librarian = new LibrarianCodeFirst()
                {
                    ID = int.Parse(l.Element("ID").Value.Trim()),
                    Phone = l.Element("Phone").Value.Trim(),
                    UserID = l.Element("UserID").Value.Trim(),
                    Password = l.Element("Password").Value.Trim()
                };

                librarians.Add(librarian);
            }
            Librarians = librarians;
        }

        /// <summary>
        /// Sets the property of PersonCodeFirst objects given a path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public void GetPersonCodeFirstsFromXDocument(string path)
        {
            //Get XElements using LINQ
            List<XElement> peopleXML = (from p in XDocument.Load(path).Descendants("Person")
                                        select p).ToList();

            //Collection to store the new people objects
            List<PersonCodeFirst> people = new List<PersonCodeFirst>(peopleXML.Count);

            PersonCodeFirst person;

            //Loop through the XElement objects and instantiate a new person and add them to the collection
            foreach (XElement p in peopleXML)
            {
                person = new PersonCodeFirst()
                {
                    PersonID = int.Parse(p.Element("PersonID").Value.Trim()),
                    FirstName = p.Element("FirstName").Value.Trim(),
                    LastName = p.Element("LastName").Value.Trim()
                };

                people.Add(person);
            }
            People = people;
        }
    }
}
