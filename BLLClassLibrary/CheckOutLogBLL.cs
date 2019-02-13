using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseFirstLibraryApp;

namespace BLLClassLibrary
{
    public class CheckOutLogBLL: IComparable<CheckOutLogBLL>
    {
        public int CheckOutLogID { get; set; }
        public int CardholderID { get; set; }
        public int BookID { get; set; }
        public DateTime CheckOutDate { get; set; }
        public Book Book { get; set; }

        public override string ToString()
        {
            return $"{CheckOutDate}";
        }

        public int CompareTo(CheckOutLogBLL other)
        {
            int output = CheckOutDate.CompareTo(other.CheckOutDate);
            if (output == 0)
            {
                return Book.Title.CompareTo(other.Book.Title);
            }
            return output;
        }

        /// <summary>
        /// Reports true if the log indicates an overdue book.
        /// </summary>
        /// <returns></returns>
        public bool IsOverDue()
        {
            return ((DateTime.Now - CheckOutDate).TotalDays > 30);
        }
    }
}
