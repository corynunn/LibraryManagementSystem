using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLClassLibrary
{
    public class CardholderBLL: PersonBLL
    {
        public string Phone { get; set; }
        public string LibraryCardID { get; set; }

        public override string ToString()
        {
            return base.ToString() + $" Card ID: {LibraryCardID} Phone: {Phone}";
        }
    }
}
