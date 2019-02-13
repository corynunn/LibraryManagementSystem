using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLClassLibrary
{
    public class AuthorBuilder
    {
        PersonBLLCollection people;

        string firstName;
        string lastName;
        string bio;
        int iD;

        public AuthorBLL Author { get; private set; }
        public bool IsNewPerson { get; set; }
        public bool IsNewAuthor { get; set; }

        public AuthorBuilder(PersonBLLCollection People, string FirstName, string LastName)
        {
            people = People;
            firstName = FirstName;
            lastName = LastName;

            //This is just for UI purposes during author creation
            Author = new AuthorBLL()
            {
                FirstName = firstName,
                LastName = lastName
            };
        }

        /// <summary>
        /// Finishes the authorBLL creation.
        /// </summary>
        public void CreateAuthor()
        {
            Author = new AuthorBLL()
            {
                FirstName = firstName,
                LastName = lastName,
                Bio = bio,
                ID = iD
            };
        }

        /// <summary>
        /// Adds the author to the personBLLCollection
        /// </summary>
        public void AddAuthorToCollection()
        {
            //CreateAuthor();
            people.Add(Author);

        }

        /// <summary>
        /// Used to set the foreign key when adding to the database.
        /// </summary>
        /// <param name="id"></param>
        public void SetID(int id)
        {
            iD = id;

        }

        /// <summary>
        /// Sets the bio for the author.
        /// </summary>
        /// <param name="Bio"></param>
        public void SetBio(string Bio)
        {
            bio = Bio;
        }


        ///// <summary>
        ///// Checks the person collection to see if the name combination is unique while adding them to the object. Will return false if an author with this name exists.
        ///// </summary>
        ///// <param name="firstName"></param>
        ///// <param name="lastName"></param>
        ///// <returns></returns>
        //public bool CheckNames(string FirstName, string LastName)
        //{
        //    firstName = FirstName;
        //    lastName = LastName;
        //    List<PersonBLL> personBLLs = (from p in people
        //                                  where p.FirstName == firstName && p.LastName == lastName
        //                                  select p).ToList();
        //    if (personBLLs.Count == 0)
        //    {
        //        //There is no one in the collection with this name, this is an entirely new person
        //        IsNewPerson = true;

        //    }
        //    else
        //    {
        //        //There is someone with this name combination, check if they are already an author
        //        foreach (PersonBLL p in personBLLs)
        //        {
        //            iD = p.ID;
        //            if (p is AuthorBLL a)
        //            {
        //                //An author with this name was found, returns false
        //                return false;
        //            }
        //        }
        //        //The person in the collection is not a cardholder yet, we can proceed.

        //    }
        //    return true;
        //}
    }
}
