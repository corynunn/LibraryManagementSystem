using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirstSeedDatabaseLibraryApp
{
    [Table("People")]
    public class PersonCodeFirst
    {
        [Key]
        public int PersonID { get; set; }
        [Index("IX_FirstNameAndLastName", IsUnique = true, Order = 0), Required,MinLength(1,ErrorMessage ="Name must be at leat 1 character."),MaxLength(30,ErrorMessage ="First name exceeds maximum length (30).")]
        public string FirstName { get; set; }
        [Index("IX_FirstNameAndLastName", IsUnique = true, Order = 1), Required, MinLength(1, ErrorMessage = "Name must be at leat 1 character."), MaxLength(50, ErrorMessage = "Last name exceeds maximum length (50).")]
        public string LastName { get; set; }
    }
}
