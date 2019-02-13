using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirstSeedDatabaseLibraryApp
{
    [Table("Librarians")]
    public class LibrarianCodeFirst
    {
        [Key]
        public int ID { get; set; }
        [Required, MaxLength(20, ErrorMessage = "Phone number exceeds maximum length (20).")]
        public string Phone { get; set; }
        [Required,MaxLength(50, ErrorMessage ="User ID exceeds maximum length(50)."), Index("UserIDPasswordIsUnique", IsUnique = true, Order = 0)]
        public string UserID { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Password exceeds maximum length(50)."), Index("UserIDPasswordIsUnique", IsUnique = true, Order = 1)]
        public string Password { get; set; }

        [ForeignKey("ID")]
        public virtual PersonCodeFirst Person { get; set; }
    }
}
