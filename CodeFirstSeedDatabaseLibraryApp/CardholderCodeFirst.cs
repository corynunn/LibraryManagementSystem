using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirstSeedDatabaseLibraryApp
{
    [Table("Cardholders")]
    public class CardholderCodeFirst
    {
        [Key]
        public int ID { get; set; }
        [Required, MaxLength(20, ErrorMessage = "Phone number exceeds maximum length (20).")]
        public string Phone { get; set; }
        [Required,MaxLength(50,ErrorMessage ="Card ID exceeds maximum length (50).")]
        public string LibraryCardID { get; set; }

        [ForeignKey("ID")]
        public virtual PersonCodeFirst Person { get; set; }
    }
}
