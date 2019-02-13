using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLClassLibrary
{
    public class AuthorSearch
    {
        PersonBLLCollection people;

        public List<PersonBLL> FoundPersons { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public AuthorBLL Author { get; private set; }

        public AuthorSearch (PersonBLLCollection People, string FirstNameQuery, string LastNameQuery)
        {
            people = People;
            FirstName = FirstNameQuery;
            LastName = LastNameQuery;
            FindAuthor();
        }

        /// <summary>
        /// Finds the Person objects and populates the fields.
        /// </summary>
        void FindAuthor()
        {
            FoundPersons = (from p in people
                           where p.FirstName == FirstName && p.LastName == LastName
                           select p).ToList();
            if (FoundPersons.Count > 0)
            {
                foreach (PersonBLL p in FoundPersons)
                {
                    if (p is AuthorBLL a)
                    {
                        //If an author is found then the Author property will hold a reference to them.
                        Author = a;
                        break;
                    }
                }
            }
        }
    }
}
