using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLClassLibrary
{
    public class LibrarianBLL: PersonBLL
    {
        public string Phone { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return base.ToString() + $" Phone: {Phone}";
        }

        public string LoginMessage()
        {
            return $"You are logged in as {LastName}, {FirstName}: {UserID}";
        }
    }
}
