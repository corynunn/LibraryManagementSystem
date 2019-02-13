using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseFirstLibraryApp;

namespace BLLClassLibrary
{
    public class CheckOutLogBLLCollection: IEnumerable<CheckOutLogBLL>
    {
        List<CheckOutLogBLL> logs = new List<CheckOutLogBLL>();

        public CheckOutLogBLL this[int index]
        {
            get
            {
                return GetLog(index);
            }
            set
            {
                logs.Add(value);
            }
        }

        /// <summary>
        /// Adds given log to the collection.
        /// </summary>
        /// <param name="log"></param>
        public void Add(CheckOutLogBLL log)
        {
            logs.Add(log);
            logs.Sort();
        }

        /// <summary>
        /// When given a specific book will return how many copies have not been checked out yet.
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public int AvailableCopies(BookBLL book)
        {
            int availableCopies = book.NumberOfCopies;
            foreach (CheckOutLogBLL l in logs)
            {
                if (l.BookID == book.BookID)
                {
                    availableCopies--;
                }
            }
            return availableCopies;
        }

        /// <summary>
        /// Populates the CheckOutLogs from the database.
        /// </summary>
        public void PopulateLogs()
        {
            using (LibraryDBEntities context = new LibraryDBEntities())
            {
                List<CheckOutLogBLL> newLogs = new List<CheckOutLogBLL>();
                CheckOutLogBLL newLog = null;
                foreach (CheckOutLog l in context.CheckOutLogs)
                {
                    newLog = new CheckOutLogBLL()
                    {
                        CheckOutLogID = l.CheckOutLogID,
                        CardholderID = l.CardholderID,
                        BookID = l.BookID,
                        Book = l.Book,
                        CheckOutDate = l.CheckOutDate
                    };
                    newLogs.Add(newLog);
                }
                logs = newLogs;
            }
            logs.Sort();
        }

        /// <summary>
        /// Takes a bookUpdater and uses it to update the book record in the database.
        /// </summary>
        /// <param name="updater"></param>
        public void UpdateBook(BookUpdater updater)
        {
            using (LibraryDBEntities context = new LibraryDBEntities())
            {
                Book book = (from b in context.Books
                             where b.BookID == updater.Book.BookID
                             select b).FirstOrDefault();
                //Check the properties on the updater to see what fields need to be altered.
                if (updater.ChangeNumberOfCopies)
                {
                    book.NumberOfCopies = updater.Book.NumberOfCopies;
                }

                context.SaveChanges();
            }
            Sort();
        }

        /// <summary>
        /// When given a CheckOutLogBLL the corresponding CheckOutLog is found in the database and removed.
        /// </summary>
        /// <param name="log"></param>
        public void RemoveLogFromDatabase(CheckOutLogBLL log)
        {
            using (LibraryDBEntities context = new LibraryDBEntities())
            {
                CheckOutLog checkOutLog = (from l in context.CheckOutLogs
                                           where l.CheckOutLogID == log.CheckOutLogID
                                           select l).FirstOrDefault();
                context.CheckOutLogs.Remove(checkOutLog);
                context.SaveChanges();
                logs.Remove(log);
            }
            Sort();
        }

        /// <summary>
        /// Adds a given CheckOutLogBLL as a CheckOutLog to the database.
        /// </summary>
        /// <param name="log"></param>
        public void AddLogToDatabase(CheckOutLogBLL log)
        {
            using (LibraryDBEntities context = new LibraryDBEntities())
            {
                CheckOutLog newLog = new CheckOutLog()
                {
                    CheckOutLogID = log.CheckOutLogID,
                    CheckOutDate = log.CheckOutDate,
                    CardholderID = log.CardholderID,
                    BookID = log.BookID
                };
                context.CheckOutLogs.Add(newLog);
                context.SaveChanges();
                logs.Add(log);
            }
            Sort();
        }

        /// <summary>
        /// Returns a list of the overdue logs.
        /// </summary>
        /// <returns></returns>
        public List<CheckOutLogBLL> GetOverdueLogs()
        {
            List<CheckOutLogBLL> logList = new List<CheckOutLogBLL>();
            foreach (CheckOutLogBLL l in logs)
            {
                if (l.IsOverDue())
                {
                    logList.Add(l);
                }
            }
            return logList;
        }

        /// <summary>
        /// Gives a list of checkoutlogs that match the cardholder
        /// </summary>
        /// <returns></returns>
        public List<CheckOutLogBLL> GetCheckOutLogBLLs(int ID)
        {
            List<CheckOutLogBLL> userLogs = new List<CheckOutLogBLL>();
            foreach (CheckOutLogBLL l in logs)
            {
                if (l.CardholderID == ID)
                {
                    userLogs.Add(l);
                }
            }
            return userLogs;
        }

        /// <summary>
        /// Sorts the logs by date.
        /// </summary>
        public void Sort()
        {
            logs.Sort();
        }

        /// <summary>
        /// Used to access log by CheckOutLogID.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        CheckOutLogBLL GetLog(int index)
        {
            for (int j = 0; j < logs.Count; j++)
            {
                if (logs[j].CheckOutLogID == index)
                {
                    return logs[j];
                }
            }
            throw new Exception("Tried to access log out of range.");
        }

        #region IEnumerable Support
        IEnumerator<CheckOutLogBLL> IEnumerable<CheckOutLogBLL>.GetEnumerator()
        {
            foreach (CheckOutLogBLL l in logs)
            {
                yield return l;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this;
        }
        #endregion
    }
}
