using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CodeFirstSeedDatabaseLibraryApp
{
    public class LibraryContextCodeFirst: DbContext
    {
        
        public LibraryContextCodeFirst() : base("name=LibraryDB") { }

        public DbSet<PersonCodeFirst> People { get; set; }
        public DbSet<BookCodeFirst> Books { get; set; }
        public DbSet<CheckOutLogCodeFirst> CheckOutLogs { get; set; }
        public DbSet<LibrarianCodeFirst> Librarians { get; set; }
        public DbSet<CardholderCodeFirst> Cardholders { get; set; }
        public DbSet<AuthorCodeFirst> Authors { get; set; }
    }
}
