using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLClassLibrary
{
    public class CardholderBuilder
    {
        PersonBLLCollection people;
        string firstName;
        string lastName;
        string phone;
        string libraryCardID;
        int iD;

        public CardholderBLL Cardholder { get; private set; }
        public bool IsNewPerson { get; private set; }

        public CardholderBuilder(PersonBLLCollection People)
        {
            people = People;
            IsNewPerson = false;
        }

        /// <summary>
        /// Finishes the cardholderBLL creation.
        /// </summary>
        public void CreateCardholder()
        {
            Cardholder = new CardholderBLL()
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                LibraryCardID = libraryCardID,
                ID = iD
            };
        }

        /// <summary>
        /// Used to set the foreign key when adding to the database.
        /// </summary>
        /// <param name="id"></param>
        public void SetID(int id)
        {
            if (Cardholder != null)
            {
                Cardholder.ID = id;
            }
        }

        /// <summary>
        /// Checks business rule of 20 character limit on phone number.
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public bool CheckPhoneNumber(string Phone)
        {
            phone = Phone;
            if (phone.Length > 20)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Adds the desired libraryCardID and checks if any cardholder already has, if one is found returns false.
        /// </summary>
        /// <param name="LibraryCardID"></param>
        /// <returns></returns>
        public bool CheckLibraryCardID(string LibraryCardID)
        {
            libraryCardID = LibraryCardID;
            if (libraryCardID.Length > 50)
            {
                //the desired id is too long
                return false;
            }
            //Check if a cardhold already exists with the desired libraryCardID
            foreach (PersonBLL p in people)
            {
                if (p is CardholderBLL c)
                {
                    if (c.LibraryCardID == libraryCardID)
                    {
                        //a cardholder was found with the desired id
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Checks the person collection to see if the name combination is unique while adding them to the object. Will return false if a Cardholder with this name exists.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public bool CheckNames(string FirstName, string LastName)
        {
            firstName = FirstName;
            lastName = LastName;
            List<PersonBLL> personBLLs = (from p in people
                                         where p.FirstName == firstName && p.LastName == lastName
                                         select p).ToList();
            if (personBLLs.Count == 0)
            {
                //There is no one in the collection with this name, this is an entirely new person
                IsNewPerson = true;
                
            }
            else
            {
                //There is someone with this name combination, check if they are already a cardholder
                foreach (PersonBLL p in personBLLs)
                {
                    iD = p.ID;
                    if (p is CardholderBLL c)
                    {
                        //A cardholder with this name was found, returns false
                        return false;
                    }
                }
                //The person in the collection is not a cardholder yet, we can proceed.
                
            }
            return true;
        }
    }
}
