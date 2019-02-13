using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirstSeedDatabaseLibraryApp
{
    [Table("Books")]
    public class BookCodeFirst
    {
        [Key]
        public int BookID { get; set; }
        [Required, MaxLength(50, ErrorMessage = "ISBN exceeds maximum length (50).")]
        public string ISBN { get; set; }
        [Required, MaxLength(50, ErrorMessage ="Title exceeds maximum length (50).")]
        public string Title { get; set; }
        [Required]
        public int AuthorID { get; set; }
        public int? NumPages { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        [MaxLength(50, ErrorMessage = "Publisher name exceeds maximum length (50).")]
        public string Publisher { get; set; }
        [MaxLength(4, ErrorMessage ="Year exceeds maximum legth (4).")]
        public string YearPublished { get; set; }
        public string Language { get; set; }
        [Required]
        public int NumberOfCopies { get; set; }

        [ForeignKey("AuthorID")]
        public virtual AuthorCodeFirst AuthorCodeFirst { get; set; }
    }
}
