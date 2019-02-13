using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLClassLibrary
{
    public class BookBLL: IComparable<BookBLL>
    {
        public int BookID { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public int AuthorID { get; set; }
        public int? NumPages { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string YearPublished { get; set; }
        public string Language { get; set; }
        public int NumberOfCopies { get; set; }
        

        public override string ToString()
        {
            string output = $"BookID: {BookID} ISBN: {ISBN} {Title}";
            if (Publisher != null)
            {
                output += $", Publisher: {Publisher}";
            }
            if (YearPublished != null)
            {
                output += $", {YearPublished}";
            }
            return output;
        }

        public int CompareTo(BookBLL other)
        {
            int output = Title.CompareTo(other.Title);
            if (output == 0)
            {
                return ISBN.CompareTo(other.ISBN);
            }
            return output;
        }

        /// <summary>
        /// Returns a detailed summary of the book for the search result pages.
        /// </summary>
        /// <returns></returns>
        public string Details(PersonBLL p, int checkedOut)
        {
            string output = $"{Title}\n" +
                            $"By: {p.FirstName} {p.LastName}\nISBN: {ISBN}\n";
            if (NumPages != null)
            {
                output += $"{NumPages}pg\n";
            }
            if (Subject != null)
            {
                output += $"Subject: {Subject}\n";
            }
            if (Description != null)
            {
                output += $"{Description}\n";
            }
            if (Publisher != null)
            {
                output += $"Published By: {Publisher}\n";
            }
            if (YearPublished != null)
            {
                output += $"Published In: {YearPublished}\n";
            }
            if (Language != null)
            {
                output += $"{Language}\n";
            }
            int availableCopies = NumberOfCopies - checkedOut;
            if (availableCopies > 0)
            {
                output += $"Availble Copies: {availableCopies}/{NumberOfCopies}";
            }
            else
            {
                output += $"NO COPIES AVAILABLE: 0/{NumberOfCopies}";
            }
            return output;
        }
    }
}
