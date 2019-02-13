using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseFirstLibraryApp;
using BLLClassLibrary;
using CodeFirstSeedDatabaseLibraryApp;
using System.Collections.Generic;

namespace LibraryManagementSystemTests
{
    [TestClass]
    public class UnitTest1
    {
        //Tests for the search function
        [TestMethod]
        public void TestSearchForWord()
        {
            //Arrange
            PersonBLLCollection people = CreatePersonBLLCollection();
            BookBLLCollection books = CreateBookCollection();

            string query = "Tom";

            //Act
            BookSearch search = new BookSearch(books, people, query);
            search.SearchAllFields();

            //Assert
            Assert.AreEqual(2, search.FoundBooks.Count);
        }

        [TestMethod]
        public void TestSearchForWordNotThere()
        {
            //Arrange
            PersonBLLCollection people = CreatePersonBLLCollection();
            BookBLLCollection books = CreateBookCollection();

            string query = "Zebra";

            //Act
            BookSearch search = new BookSearch(books, people, query);
            search.SearchAllFields();

            //Assert
            Assert.AreEqual(0, search.FoundBooks.Count);
        }

        [TestMethod]
        public void TestSearchForOnlyISBN()
        {
            //Arrange
            PersonBLLCollection people = CreatePersonBLLCollection();
            BookBLLCollection books = CreateBookCollection();

            string query = "4004";

            //Act
            BookSearch search = new BookSearch(books, people, query);
            search.ISBNSearch();

            //Assert
            Assert.AreEqual(1, search.FoundBooks.Count);
        }

        [TestMethod]
        public void TestSearchAllFieldsForISBN()
        {
            //Arrange
            PersonBLLCollection people = CreatePersonBLLCollection();
            BookBLLCollection books = CreateBookCollection();

            string query = "3003";
            //Act
            BookSearch search = new BookSearch(books, people, query);
            search.SearchAllFields();

            //Assert
            Assert.AreEqual(1, search.FoundBooks.Count);
        }

        //Tess for the Available Copies Function
        [TestMethod]
        public void TestAvailableCopiesOfCheckedOutBook()
        {
            //Arrange
            CheckOutLogBLLCollection logs = CreateCheckOutLogCollection();
            BookBLLCollection books = CreateBookCollection();

            int availableCopies;

            //Act
            availableCopies = logs.AvailableCopies(books[1]);

            //Assert
            Assert.AreEqual(1, availableCopies);
        }

        [TestMethod]
        public void TestAvailableCopiesOfBookNotCheckedOut()
        {
            //Arrange
            CheckOutLogBLLCollection logs = CreateCheckOutLogCollection();
            BookBLLCollection books = CreateBookCollection();

            int availableCopies;

            //Act
            availableCopies = logs.AvailableCopies(books[3]);

            //Assert
            Assert.AreEqual(books[3].NumberOfCopies, availableCopies);
        }

        //Test XML Files
        [TestMethod]
        public void TestXMLPeople()
        {
            //Arrange
            int databaseRows = 0;
            int xmlObjects;
            string path;
            XMLListRetriever lists = new XMLListRetriever();

            //Act
            path = XMLFileFinder.FindPath("People.xml");
            lists.GetPersonCodeFirstsFromXDocument(path);
            xmlObjects = lists.People.Count;

            using (LibraryDBEntities context = new LibraryDBEntities())
            {
                foreach (Person p in context.People)
                {
                    databaseRows++;
                }

            }

            //Assert
            Assert.AreEqual(databaseRows, xmlObjects);
        }

        [TestMethod]
        public void TestXMLCheckOutLog()
        {
            //Arrange
            int databaseRows = 0;
            int xmlObjects;
            string path;
            XMLListRetriever lists = new XMLListRetriever();

            //Act
            path = XMLFileFinder.FindPath("CheckOutLog.xml");
            lists.GetCheckOutLogCodeFirstsFromXDocument(path);
            xmlObjects = lists.Logs.Count;

            using (LibraryDBEntities context = new LibraryDBEntities())
            {
                foreach (CheckOutLog c in context.CheckOutLogs)
                {
                    databaseRows++;
                }

            }

            //Assert
            Assert.AreEqual(databaseRows, xmlObjects);
        }

        [TestMethod]
        public void TestXMLLibrarians()
        {
            //Arrange
            int databaseRows = 0;
            int xmlObjects;
            string path;
            XMLListRetriever lists = new XMLListRetriever();

            //Act
            path = XMLFileFinder.FindPath("Librarians.xml");
            lists.GetLibrarianCodeFirstsFromXDocument(path);
            xmlObjects = lists.Librarians.Count;

            using (LibraryDBEntities context = new LibraryDBEntities())
            {
                foreach (Librarian l in context.Librarians)
                {
                    databaseRows++;
                }

            }

            //Assert
            Assert.AreEqual(databaseRows, xmlObjects);
        }

        [TestMethod]
        public void TestXMLAuthors()
        {
            //Arrange
            int databaseRows = 0;
            int xmlObjects;
            string path;
            XMLListRetriever lists = new XMLListRetriever();

            //Act
            path = XMLFileFinder.FindPath("Authors.xml");
            lists.GetAuthorCodeFirstsFromXDocument(path);
            xmlObjects = lists.Authors.Count;

            using (LibraryDBEntities context = new LibraryDBEntities())
            {
                foreach (Author a in context.Authors)
                {
                    databaseRows++;
                }

            }

            //Assert
            Assert.AreEqual(databaseRows, xmlObjects);
        }

        [TestMethod]
        public void TestXMLBooks()
        {
            //Arrange
            int databaseRows = 0;
            int xmlObjects;
            string path;
            XMLListRetriever lists = new XMLListRetriever();

            //Act
            path = XMLFileFinder.FindPath("Books.xml");
            lists.GetBookCodeFirstsFromXDocument(path);
            xmlObjects = lists.Books.Count;

            using (LibraryDBEntities context = new LibraryDBEntities())
            {
                foreach (Book b in context.Books)
                {
                    databaseRows++;
                }

            }

            //Assert
            Assert.AreEqual(databaseRows, xmlObjects);
        }

        [TestMethod]
        public void TestXMLCardholders()
        {
            //Arrange
            int databaseRows = 0;
            int xmlObjects;
            string path;
            XMLListRetriever lists = new XMLListRetriever();

            //Act
            path = XMLFileFinder.FindPath("Cardholders.xml");
            lists.GetCardholderCodeFirstsFromXDocument(path);
            xmlObjects = lists.Cardholders.Count;

            using (LibraryDBEntities context = new LibraryDBEntities())
            {
                foreach (Cardholder c in context.Cardholders)
                {
                    databaseRows++;
                }

            }

            //Assert
            Assert.AreEqual(databaseRows, xmlObjects);
        }

        //Helper Methods
        CheckOutLogBLLCollection CreateCheckOutLogCollection()
        {
            CheckOutLogBLLCollection logs = new CheckOutLogBLLCollection();

            Book book1 = new Book()
            {
                BookID = 1,
                Title = "Tom Goes To The Doctor"
            };
            Book book2 = new Book()
            {
                BookID = 2,
                Title = "Mary Goes To The Dentist"
            };

            CheckOutLogBLL l1 = new CheckOutLogBLL()
            {
                CheckOutLogID = 1,
                BookID = 1,
                CheckOutDate = DateTime.Now,
                Book = book1
            };
            CheckOutLogBLL l2 = new CheckOutLogBLL()
            {
                CheckOutLogID = 2,
                BookID = 1,
                CheckOutDate = DateTime.Now,
                Book = book1
            };
            CheckOutLogBLL l3 = new CheckOutLogBLL()
            {
                CheckOutLogID = 3,
                BookID = 2,
                CheckOutDate = DateTime.Now,
                Book = book2
            };

            logs.Add(l1);
            logs.Add(l2);
            logs.Add(l3);

            return logs;
        }

        PersonBLLCollection CreatePersonBLLCollection()
        {
            PersonBLLCollection people = new PersonBLLCollection();
            AuthorBLL author1 = new AuthorBLL()
            {
                ID = 1,
                FirstName = "Sam",
                LastName = "Jacobs"
            };
            AuthorBLL author2 = new AuthorBLL()
            {
                ID = 2,
                FirstName = "Fran",
                LastName = "Heathwood"
            };
            AuthorBLL author3 = new AuthorBLL()
            {
                ID = 3,
                FirstName = "Tom",
                LastName = "Marrows"
            };
            AuthorBLL author4 = new AuthorBLL()
            {
                ID = 4,
                FirstName = "Karen",
                LastName = "Flint"
            };

            people.Add(author1);
            people.Add(author2);
            people.Add(author3);
            people.Add(author4);

            return people;
        }


        BookBLLCollection CreateBookCollection()
        {
            BookBLLCollection books = new BookBLLCollection();

            BookBLL book1 = new BookBLL()
            {
                BookID = 1,
                ISBN = "1001",
                Title = "Tom Goes To The Doctor",
                AuthorID = 1,
                NumberOfCopies = 3
            };
            BookBLL book2 = new BookBLL()
            {
                BookID = 2,
                ISBN = "2002",
                Title = "Mary Goes To The Dentist",
                AuthorID = 2,
                NumberOfCopies = 3
            };
            BookBLL book3 = new BookBLL()
            {
                BookID = 3,
                ISBN = "3003",
                Title = "Max Goes To The Barber",
                AuthorID = 3,
                NumberOfCopies = 3
            };
            BookBLL book4 = new BookBLL()
            {
                BookID = 4,
                ISBN = "4004",
                Title = "Susan Goes To The Cabal",
                AuthorID = 4,
                NumberOfCopies = 3
            };

            books.Add(book1);
            books.Add(book2);
            books.Add(book3);
            books.Add(book4);

            return books;
        }
    }
}
