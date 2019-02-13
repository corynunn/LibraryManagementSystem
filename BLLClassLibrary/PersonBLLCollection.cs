using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseFirstLibraryApp;

namespace BLLClassLibrary
{
    public class PersonBLLCollection : IEnumerable<PersonBLL>
    {
        List<PersonBLL> people = new List<PersonBLL>();

        public PersonBLL this[int index]
        {
            get
            {
                return GetPerson(index);
            }
            set
            {
                people.Add(value);
            }
        }

        

        /// <summary>
        /// Creates a list of business logic layer objects from the database tables
        /// </summary>
        public void PopulatePeople()
        {
            using (LibraryDBEntities context = new LibraryDBEntities())
            {
                List<PersonBLL> newPeople = new List<PersonBLL>();
                //Add the librarians
                LibrarianBLL librarian = null;
                foreach (Librarian l in context.Librarians)
                {
                    librarian = new LibrarianBLL()
                    {
                        ID = l.Person.PersonID,
                        FirstName = l.Person.FirstName,
                        LastName = l.Person.LastName,
                        Phone = l.Phone,
                        UserID = l.UserID,
                        Password = l.Password
                    };
                    newPeople.Add(librarian);
                }
                //Add the authors
                AuthorBLL author = null;
                foreach (Author a in context.Authors)
                {
                    author = new AuthorBLL()
                    {
                        ID = a.Person.PersonID,
                        FirstName = a.Person.FirstName,
                        LastName = a.Person.LastName,
                        Bio = a.Bio
                    };
                    newPeople.Add(author);
                }
                //Add cardholders
                CardholderBLL cardholder = null;
                foreach (Cardholder c in context.Cardholders)
                {
                    cardholder = new CardholderBLL()
                    {
                        ID = c.Person.PersonID,
                        FirstName = c.Person.FirstName,
                        LastName = c.Person.LastName,
                        Phone = c.Phone,
                        LibraryCardID = c.LibraryCardID
                    };
                    newPeople.Add(cardholder);
                }
                people = newPeople;
            }
            Sort();
        }

        /// <summary>
        /// Returns the ID of a person when given their first and last names, if no person is found will return -1.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public int GetPersonID(string firstName, string lastName)
        {
            PersonBLL person = (from p in people
                               where p.FirstName == firstName && p.LastName == lastName
                               select p).FirstOrDefault();
            if (person == null)
            {
                return -1;
            }
            else
            {
                return person.ID;
            }
        }

        /// <summary>
        /// Uses the cardholderbuilder to create a cardholder, and possibly a new person as well.
        /// </summary>
        /// <param name="builder"></param>
        public void AddCardholderToDatabase(CardholderBuilder builder)
        {
            using (LibraryDBEntities context = new LibraryDBEntities())
            {
                //If this is a new person we will need to add them to the People table
                if (builder.IsNewPerson)
                {
                    Person p = new Person()
                    {
                        FirstName = builder.Cardholder.FirstName,
                        LastName = builder.Cardholder.LastName
                    };
                    context.People.Add(p);
                    context.SaveChanges();

                    //To add the cardholder we need the foreign key
                    Person person = (from x in context.People
                                    where x.FirstName == builder.Cardholder.FirstName && x.LastName == builder.Cardholder.LastName
                                    select x).FirstOrDefault();
                    builder.SetID(person.PersonID);
                }
                //Now the cardholder object is created and added to the Cardholders table
                Cardholder c = new Cardholder()
                {
                    ID = builder.Cardholder.ID,
                    Phone = builder.Cardholder.Phone,
                    LibraryCardID = builder.Cardholder.LibraryCardID
                };
                context.Cardholders.Add(c);
                context.SaveChanges();
            }
            PopulatePeople();
        }

        /// <summary>
        /// Sorts base on name.
        /// </summary>
        void Sort()
        {
            people.Sort();
        }

        /// <summary>
        /// Adds a person to the collection.
        /// </summary>
        /// <param name="person"></param>
        public void Add(PersonBLL person)
        {
            people.Add(person);
            Sort();
        }

        /// <summary>
        /// Finds the next available ID.
        /// </summary>
        /// <returns></returns>
        public int NextAvailableID()
        {
            int output = 0;
            PersonBLL person = null;
            do
            {
                output++;
                person = (from p in people
                          where p.ID == output
                          select p).FirstOrDefault();
            } while (person != null);
            return output;
        }

        /// <summary>
        /// Adds a given author to the database, if there isn't a person entry already it will be created.
        /// </summary>
        /// <param name="author"></param>
        public void AddAuthorToDatabase(AuthorBLL author)
        {
            using (LibraryDBEntities context = new LibraryDBEntities())
            {
                Person person = (from p in context.People
                                 where p.PersonID == author.ID
                                 select p).FirstOrDefault();
                if (person == null)
                {
                    person = new Person()
                    {
                        PersonID = author.ID,
                        FirstName = author.FirstName,
                        LastName = author.LastName
                    };
                    context.People.Add(person);
                }

                Author newAuthor = new Author()
                {
                    ID = author.ID,
                    Bio = author.Bio
                };
                context.Authors.Add(newAuthor);
                context.SaveChanges();
            }
            PopulatePeople();
        }

        /// <summary>
        /// Returns a personBLL matching a given ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        PersonBLL GetPerson(int ID)
        {
            for (int k = 0; k < people.Count; k++)
            {
                if (people[k].ID == ID)
                {
                    return people[k];
                }
            }
            throw new Exception("No person found matching that ID.");
        }

        #region IEnumerable Support
        IEnumerator<PersonBLL> IEnumerable<PersonBLL>.GetEnumerator()
        {
            foreach (PersonBLL p in people)
            {
                yield return p;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this;
        }
        #endregion
    }
}
