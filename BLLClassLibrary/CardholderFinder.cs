using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLClassLibrary
{
    public class CardholderFinder
    {
        PersonBLLCollection people;

        public CardholderBLL Cardholder { get; private set; }
        public string LibraryCardID { get; private set; }

        public CardholderFinder(PersonBLLCollection People, string CardID)
        {
            people = People;
            LibraryCardID = CardID;

            FindCardholder();
        }

        /// <summary>
        /// Attempts to find the cardholder matching the provided string.
        /// </summary>
        void FindCardholder()
        {
            foreach (PersonBLL p in people)
            {
                if (p is CardholderBLL c)
                {
                    if (c.LibraryCardID == LibraryCardID)
                    {
                        Cardholder = c;
                        break;
                    }
                }
            }
        }
    }
}
