using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirstSeedDatabaseLibraryApp
{
    [Table("CheckOutLogs")]
    public class CheckOutLogCodeFirst
    {
        [Key]
        public int CheckOutLogID { get; set; }
        [Required]
        public int CardholderID { get; set; }
        [Required]
        public int BookID { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }

        [ForeignKey("CardholderID")]
        public virtual CardholderCodeFirst Cardholder { get; set; }
        [ForeignKey("BookID")]
        public virtual BookCodeFirst Book { get; set; }
    }
}
