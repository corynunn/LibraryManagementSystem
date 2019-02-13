using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirstSeedDatabaseLibraryApp
{
    [Table("Authors")]
    public class AuthorCodeFirst
    {
        [Key]
        public int ID { get; set; }
        public string Bio { get; set; }

        [ForeignKey("ID")]
        public virtual PersonCodeFirst Person { get; set; }
    }
}
