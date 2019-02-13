using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLClassLibrary
{
    public abstract class PersonBLL: IComparable<PersonBLL>
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }



        public override string ToString()
        {
            string output = $"ID: {ID}: {FirstName} {LastName}";
            return output;
        }

        public int CompareTo(PersonBLL other)
        {
            int output = LastName.CompareTo(other.LastName);
            if (output == 0)
            {
                return FirstName.CompareTo(other.FirstName);
            }
            return output;
        }
    }
}
