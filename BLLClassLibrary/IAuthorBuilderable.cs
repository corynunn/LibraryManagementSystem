using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLClassLibrary
{
    public interface IAuthorBuilderable
    {
        //Used to add authors to the database
        BookBLL Book { get; set; }
        AuthorBuilder AuthorBuilder { get; set; }
        void SetBio(string bio);
    }
}
