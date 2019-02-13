using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CodeFirstSeedDatabaseLibraryApp
{
    static public class CreateCodeFirstDatabase
    {
        /// <summary>
        /// Uses LibraryContext and LibraryContextInitializer to create a new Code First database.
        /// </summary>
        public static void CreateAndInitializeDatabase()
        {
            //Sets the initializer for the database
            Database.SetInitializer<LibraryContextCodeFirst>(new LibraryContextInitializer());

            //To create the database we must try to access it. Below the code will grab the first book but not do anything with it.
            using (LibraryContextCodeFirst context = new LibraryContextCodeFirst())
            {
                BookCodeFirst book = (from b in context.Books
                             select b).FirstOrDefault();
                //At this point the database will be made from Code First, and concludes the Code First contribution to this app.
            }
        }
    }
}
