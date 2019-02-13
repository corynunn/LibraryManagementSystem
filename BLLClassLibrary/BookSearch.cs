using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLClassLibrary
{
    public class BookSearch
    {
        BookBLLCollection books;
        PersonBLLCollection people;
        List<string> splitQuery;

        public List<BookBLL> FoundBooks { get; private set; }
        public string QueryTerms { get; private set; }

        public BookSearch(BookBLLCollection Books, PersonBLLCollection People, string Query)
        {
            books = Books;
            people = People;
            QueryTerms = Query;

            //When this class is created it splits the query
            splitQuery = SplitQueryTerms(QueryTerms);
        }

        /// <summary>
        /// Splits the search into a list of strings.
        /// </summary>
        /// <param name="queryTerms"></param>
        /// <returns></returns>
        List<string> SplitQueryTerms(string queryTerms)
        {
            char[] splitter = { ' ' };
            List<string> searchQuery = queryTerms.Split(splitter, StringSplitOptions.RemoveEmptyEntries).ToList();
            return searchQuery;
        }

        /// <summary>
        /// Takes each book and creates a list of strings from the fields to be searched for.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        List<string> ConcatenateBookFields(BookBLL b)
        {
            List<string> concatenatedFields = new List<string>();

            concatenatedFields.Add(b.Title);

            if (b.Subject != null)
            {
                concatenatedFields.Add(b.Subject);
            }

            PersonBLL author = (from a in people
                                where a.ID == b.AuthorID
                                select a).FirstOrDefault();

            concatenatedFields.Add(author.FirstName);
            concatenatedFields.Add(author.LastName);

            return concatenatedFields;
        }

        /// <summary>
        /// Searches book fields for the search terms.
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="queries"></param>
        /// <returns></returns>
        bool StringAndSearch(List<string> dataSource, List<string> queries)
        {

            foreach (string q in queries)
            {
                bool MatchFound = false;
                foreach (string str in dataSource)
                {

                    if (str.ToLower().Contains(q.Trim().ToLower()))
                    {
                        MatchFound = true;
                        break;
                    }
                }
                if (MatchFound == false)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Searches the book collection for a book with an ISBN that matches the entered string.
        /// </summary>
        public void ISBNSearch()
        {
            FoundBooks = new List<BookBLL>();

            BookBLL book = (from b in books
                            where b.ISBN == QueryTerms
                            select b).FirstOrDefault();
            if (book != null)
            {
                FoundBooks.Add(book);
            }
        }

        /// <summary>
        /// Will first run an ISBN search, if that fails to find a record then the other fields will be searched.
        /// </summary>
        public void SearchAllFields()
        {
            ISBNSearch();

            if (FoundBooks.Count == 0)
            {
                foreach (BookBLL b in books)
                {
                    List<string> concatenatedFields = ConcatenateBookFields(b);
                    if (StringAndSearch(concatenatedFields, splitQuery))
                    {
                        FoundBooks.Add(b);
                    }
                }
                if (FoundBooks.Count > 1)
                {
                    FoundBooks.Sort();
                }
            }
        }
    }
}
